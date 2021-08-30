using System;
using UnityEngine;

namespace ET
{
    public static class AssetEntityPoolSystem
    {
        
        /// <summary>
        /// 取出实例化gameObject
        /// </summary>
        public static GameObject FetchGameObject(this AssetEntityPool self, Transform parent = null)
        {
            if (self.RefCount==0)
            {
                self.RemoveComponent<AssetEntityPoolCountDownComponent>();
            }
            self.LastUseObjectTime = TimeHelper.ServerNow();
            self.RefCount++;
            if (self.Pool.Count==0)
            {
                var obj = UnityEngine.Object.Instantiate(self.GameObjectRes, parent);
                return obj;
            }
            else
            {
                var obj =  self.Pool.Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(parent);
                return obj;
            }
        }

        /// <summary>
        /// 回收实例化gameObject
        /// </summary>
        public static void RecycleGameObject(this AssetEntityPool self, GameObject gameObject)
        {
            if (self==null)
            {
                return;
            }
            self.LastUseObjectTime = TimeHelper.ServerNow();
            self.RefCount--;
            gameObject.transform.SetParent(PoolingAssetComponent.Instance.AssetPoolTransform);
            gameObject.SetActive(false);
            self.Pool.Enqueue(gameObject);
            if (self.RefCount==0)
            {
                int assetPoolRecycleMillsoconds = self.CachePoolMillSeconds;
                if (assetPoolRecycleMillsoconds<=0)
                {
                    assetPoolRecycleMillsoconds = FrameworkConfigVar.AssetPoolRecycleMillSeconds.IntVar();
                }
                long DisposeTime = TimeHelper.ClientNow() + assetPoolRecycleMillsoconds;
                self.AddComponent<AssetEntityPoolCountDownComponent, long>(DisposeTime);
            }
        }

        /// <summary>
        /// 释放未使用的缓存GameObject
        /// 保留至指定的个数
        /// </summary>
        public static void ReleaseUnUseObject(this AssetEntityPool self, int maxObjectCount = 0)
        {
            while (self.Pool.Count>maxObjectCount)
            {
                UnityEngine.Object.Destroy(self.Pool.Dequeue());
            }
        }
    }
    
    
    public class AssetEntityPoolAwakeSystem : AwakeSystem<AssetEntityPool, GameObject, string, string ,int>
    {
        public override void Awake(AssetEntityPool self, GameObject gameObjectRes, string bundleName, string assetPath, int cachePoolMillSeconds)
        {
            self.GameObjectRes = gameObjectRes;
            self.BundleName = bundleName;
            self.AssetPath = assetPath;
            self.CachePoolMillSeconds = cachePoolMillSeconds;
            self.LastUseObjectTime = TimeHelper.ServerNow();
        }
    }
    
    public class AssetEntityPoolUpdateSystem : UpdateSystem<AssetEntityPool>
    {
        public override void Update(AssetEntityPool self)
        {
            if (TimeHelper.ServerNow()-self.LastUseObjectTime> 5000 && self.Pool.Count>0)
            {
                self.ReleaseUnUseObject();
            }
        }
    }

    public class AssetEntityPoolDestroySystem : DestroySystem<AssetEntityPool>
    {
        public override void Destroy(AssetEntityPool self)
        {
            while (self.Pool.Count>0)
            {
                UnityEngine.Object.Destroy(self.Pool.Dequeue());
            }
            self.GameObjectRes = null;
            self.RefCount = 0;
            self.LastUseObjectTime = 0;
            if (ResourcesComponent.Instance==null)
            {
                return;
            }
            ResourcesComponent.Instance.UnloadBundle(self.BundleName);
            if (self.Parent is PoolingAssetComponent poolingAssetComponent)
            {
                var value =  poolingAssetComponent.PathAssetEntityPools.Remove(self.AssetPath);
                Log.Debug($"{self.AssetPath} 卸载结果 {value.ToString()}");
            }
            else
            {
                Log.Error($"AssetEntityPool的父节点不是PoolingAssetComponent！");
            }
            self.AssetPath = null;
            self.CachePoolMillSeconds = 0;
        }
    }
}