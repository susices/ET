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
                    T entity = await Game.Scene.GetComponent<DBComponent>().Query<T>(self.DomainZone(), playerId);
                    self.AddCacheData(playerId, entity);
                    return entity;
                }

                if (!self.UnitCaches[playerId].ContainsKey(typeof (T)))
                {
                    T entity = await Game.Scene.GetComponent<DBComponent>().Query<T>(self.DomainZone(), playerId);
                    self.UnitCaches[playerId].Add(typeof (T), entity);
                    self.MoveCacheToHead(playerId);
                    return entity;
                }
                self.MoveCacheToHead(playerId);
                Entity cacheEntity = self.UnitCaches[playerId][typeof (T)];
                return cacheEntity as T;
            }
        }

        public static async ETTask Save<T>(this DBCacheComponent self, long playerId, T entity) where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
                if (!self.UnitCaches.ContainsKey(playerId))
                {
                    self.AddCacheData(playerId, entity);
                    return;
                }
                
                self.UpdateCacheData(playerId,entity);
            }
        }

        public static async ETTask Save(this DBCacheComponent self, long playerId, List<Entity> entities)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
            }
        }

        public static void AddCacheData<T>(this DBCacheComponent self, long playerId, T entity) where T : Entity
        {
            if (self.UnitCaches.Count >= self.LRUCapacity)
            {
                self.ClearPlayerCache(self.TailCacheNode.Id);
            }

            var dic = self.UnitCachePool.Fetch();
            dic.Add(typeof (T), entity);
            self.UnitCaches.Add(playerId, dic);
            self.AddCacheNode(playerId);
        }

        public static void UpdateCacheData<T>(this DBCacheComponent self, long playerId, T entity) where T : Entity
        {
            if (!self.UnitCaches[playerId].ContainsKey(typeof (T)))
            {
                self.UnitCaches[playerId].Add(typeof (T), entity);
            }
            else
            {
                self.UnitCaches[playerId][typeof (T)] = entity;
            }
            self.MoveCacheToHead(playerId);
        }

        /// <summary>
        /// 添加缓存节点
        /// 会添加至节点链表头
        /// </summary>
        public static void AddCacheNode(this DBCacheComponent self, long playerId)
        {
            LRUCacheNode cacheNode = self.CacheNodePool.Fetch();
            cacheNode.Id = playerId;
            self.LruCacheNodes.Add(playerId, cacheNode);
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
            //Log.Info($"添加节点  playerId:{playerId.ToString()}");
        }

        /// <summary>
        /// 移动缓存节点到最前
        /// </summary>
        public static void MoveCacheToHead(this DBCacheComponent self, long playerId)
        {
            if (!self.LruCacheNodes.ContainsKey(playerId))
            {
                Log.Error($"DBCache 未找到 cacheNode playerId:{playerId.ToString()}");
                return;
            }

            // 已经是头节点 跳过
            if (self.HeadCacheNode.Id == playerId)
            {
                return;
            }

            LRUCacheNode cacheNode = self.LruCacheNodes[playerId];
            //尾节点 
            if (self.TailCacheNode.Id == playerId)
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
            //Log.Info($"移动至头节点  playerId:{playerId.ToString()}");
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
            //Log.Info($"清除节点  playerId:{playerId.ToString()}");
        }
    }
}