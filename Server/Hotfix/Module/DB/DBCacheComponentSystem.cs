using System.Collections.Generic;

namespace ET
{
    public class DBCacheComponentAwakeSystem: AwakeSystem<DBCacheComponent>
    {
        public override void Awake(DBCacheComponent self)
        {
            self.LRUCapacity = FrameworkConfigVar.LRUCapacity.IntVar();
        }
    }

    public static class DBCacheComponentSystem
    {
        public static async ETTask<T> Query<T>(this DBCacheComponent self, long playerId) where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
                if (!self.UnitCaches.ContainsKey(playerId))
                {
                    if (self.UnitCaches.Count >= self.LRUCapacity)
                    {
                        self.ClearPlayerCache(self.TailCacheNode.PlayerId);
                    }

                    Log.Info($"当前节点数： {self.UnitCaches.Count.ToString()} 总容量： {self.LRUCapacity.ToString()}");

                    T entity = await Game.Scene.GetComponent<DBComponent>().Query<T>(self.DomainZone(), playerId);
                    var dic = self.UnitCachePool.Fetch();
                    dic.Add(typeof (T), entity);
                    self.UnitCaches.Add(playerId, dic);
                    self.AddCacheNode(playerId);
                    Log.Info($"查询playerId:{playerId.ToString()} 数据类型：{typeof (T).Name} 添加缓存节点");
                    return entity;
                }

                if (!self.UnitCaches[playerId].ContainsKey(typeof (T)))
                {
                    T entity = await Game.Scene.GetComponent<DBComponent>().Query<T>(self.DomainZone(), playerId);
                    self.UnitCaches[playerId].Add(typeof (T), entity);
                    await self.MoveCacheToHead(playerId);
                    Log.Info($"查询playerId:{playerId.ToString()} 数据类型：{typeof (T).Name} 缓存节点添加类型 移位至头节点");
                    return entity;
                }

                Entity cacheEntity = self.UnitCaches[playerId][typeof (T)];
                await self.MoveCacheToHead(playerId);
                Log.Info($"查询playerId:{playerId.ToString()} 数据类型：{typeof (T).Name} 缓存节点找到类型 移位至头节点");
                return cacheEntity as T;
            }
        }

        public static async ETTask Save<T>(this DBCacheComponent self, long playerId, T Entity) where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
                if (!self.UnitCaches.ContainsKey(playerId))
                {
                    if (self.UnitCaches.Count >= self.LRUCapacity)
                    {
                        self.ClearPlayerCache(self.TailCacheNode.PlayerId);
                    }

                    var dic = self.UnitCachePool.Fetch();
                    dic.Add(typeof (T), Entity);
                    self.UnitCaches.Add(playerId, dic);
                    self.AddCacheNode(playerId);
                    return;
                }

                if (!self.UnitCaches[playerId].ContainsKey(typeof (T)))
                {
                    self.UnitCaches[playerId].Add(typeof (T), Entity);
                }
                else
                {
                    self.UnitCaches[playerId][typeof (T)] = Entity;
                }

                await self.MoveCacheToHead(playerId);
            }
        }

        public static async ETTask Save(this DBCacheComponent self, long playerId, List<Entity> entities)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
            }
        }

        /// <summary>
        /// 添加缓存节点到最前
        /// </summary>
        public static void AddCacheNode(this DBCacheComponent self, long playerId)
        {
            LRUCacheNode cacheNode = self.CacheNodePool.Fetch();
            cacheNode.PlayerId = playerId;
            self.LruCacheNodes.Add(playerId,cacheNode);
            LRUCacheNode headCacheNode = self.HeadCacheNode;
            if (headCacheNode != null)
            {
                headCacheNode.Pre = cacheNode;
                cacheNode.Next = headCacheNode;
            }
            else
            {
                self.TailCacheNode = cacheNode;
            }

            self.HeadCacheNode = cacheNode;
        }

        /// <summary>
        /// 移动缓存节点到最前
        /// </summary>
        public static async ETTask MoveCacheToHead(this DBCacheComponent self, long playerId)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
                if (!self.LruCacheNodes.ContainsKey(playerId))
                {
                    Log.Error($"DBCache 未找到 cacheNode playerId:{playerId.ToString()}");
                    return;
                }

                // 已经是头节点 跳过
                if (self.HeadCacheNode.PlayerId == playerId)
                {
                    return;
                }

                LRUCacheNode cacheNode = self.LruCacheNodes[playerId];
                //尾节点 
                if (self.TailCacheNode.PlayerId == playerId)
                {
                    self.TailCacheNode = cacheNode.Pre;
                    self.TailCacheNode.Next = null;
                    LRUCacheNode oldHeadNode = self.HeadCacheNode;
                    self.HeadCacheNode = cacheNode;
                    self.HeadCacheNode.Pre = null;
                    self.HeadCacheNode.Next = oldHeadNode;
                    oldHeadNode.Pre = self.HeadCacheNode;
                }
                //中间节点
                else
                {
                    LRUCacheNode preNode = cacheNode.Pre;
                    LRUCacheNode nextNode = cacheNode.Next;
                    preNode.Next = nextNode;
                    nextNode.Pre = preNode;
                    LRUCacheNode oldHeadNode = self.HeadCacheNode;
                    self.HeadCacheNode = cacheNode;
                    self.HeadCacheNode.Pre = null;
                    self.HeadCacheNode.Next = oldHeadNode;
                    oldHeadNode.Pre = self.HeadCacheNode;
                }
            }
        }

        /// <summary>
        /// 清除指定player的缓存节点和数据
        /// </summary>
        public static void ClearPlayerCache(this DBCacheComponent self, long playerId)
        {
            if (!self.LruCacheNodes.ContainsKey(playerId))
            {
                return;
            }

            LRUCacheNode cacheNode = self.LruCacheNodes[playerId];
            if (cacheNode.Next == null)
            {
                // 尾节点 设置前一个为尾节点
                if (cacheNode.Pre != null)
                {
                    LRUCacheNode preNode = cacheNode.Pre;
                    preNode.Next = null;
                    self.TailCacheNode = preNode;
                }
                else
                {
                    self.HeadCacheNode = null;
                    self.TailCacheNode = null;
                }
            }
            else
            {
                // 中间节点  连接前后节点
                if (cacheNode.Pre != null)
                {
                    cacheNode.Pre.Next = cacheNode.Next;
                    cacheNode.Next.Pre = cacheNode.Pre;
                }
                else
                {
                    self.HeadCacheNode = cacheNode.Next;
                    self.HeadCacheNode.Pre = null;
                }
            }

            self.LruCacheNodes.Remove(playerId);
            cacheNode.Clear();
            self.CacheNodePool.Recycle(cacheNode);
            var dic = self.UnitCaches[playerId];
            self.UnitCaches.Remove(playerId);
            dic.Clear();
            self.UnitCachePool.Recycle(dic);
            Log.Info($"清除缓存  playerId:{playerId.ToString()}");
        }
    }
}