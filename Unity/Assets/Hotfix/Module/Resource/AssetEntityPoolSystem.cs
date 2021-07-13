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
            self.RefCount--;
            gameObject.transform.SetParent(PoolingAssetComponent.Instance.AssetPoolTransform);
            gameObject.SetActive(false);
            self.Pool.Enqueue(gameObject);
            if (self.RefCount==0)
            {
                int assetPoolRecycleMillsoconds = LocalizationArtAssetCategory.Instance.Get(self.AssetPathIndex).CachePoolMillSeconds;
                if (assetPoolRecycleMillsoconds<=0)
                {
                    assetPoolRecycleMillsoconds = FrameworkConfigVar.AssetPoolRecycleMillSeconds.IntVar();
                }
                self.DisposeTime = TimeHelper.ClientNow() + assetPoolRecycleMillsoconds;
            }
        }
    }
    
    
    public class AssetEntityPoolAwakeSystem : AwakeSystem<AssetEntityPool, GameObject, string, int>
    {
        public override void Awake(AssetEntityPool self, GameObject gameObjectRes, string bundleName, int assetPathIndex)
        {
            self.GameObjectRes = gameObjectRes;
            self.BundleName = bundleName;
            self.AssetPathIndex = assetPathIndex;
        }
    }
    
    public class AssetEntityPoolUpdateSystem : UpdateSystem<AssetEntityPool>
    {
        public override void Update(AssetEntityPool self)
        {
            if (self.RefCount==0 && TimeHelper.ClientNow() > self.DisposeTime)
            {
                self.Dispose();
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
            self.DisposeTime = 0;
            if (ResourcesComponent.Instance==null)
            {
                return;
            }
            ResourcesComponent.Instance.UnloadBundle(self.BundleName);
            if (self.Parent is PoolingAssetComponent poolingAssetComponent)
            {
                var value =  poolingAssetComponent.PathAssetEntityPools.Remove(self.AssetPathIndex.LocalizedAssetPath());
                Log.Debug($"{self.AssetPathIndex.LocalizedAssetPath()} 卸载结果 {value.ToString()}");
            }
            else
            {
                Log.Error($"AssetEntityPool的父节点不是PoolingAssetComponent！");
            }
            self.AssetPathIndex = 0;
        }
    }
}