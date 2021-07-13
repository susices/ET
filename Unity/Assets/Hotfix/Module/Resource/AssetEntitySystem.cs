using UnityEngine;

namespace ET
{
    public class AssetEntityAwakeSystem : AwakeSystem<AssetEntity,string, Transform>
    {
        public override void Awake(AssetEntity self,string assetPath , Transform parent)
        {
            if (PoolingAssetComponent.Instance.PathAssetEntityPools.TryGetValue(assetPath, out var assetEntityPool))
            {
                self.Awake(assetPath, assetEntityPool.FetchGameObject(parent));
            }
        }
    }
    
    public class AssetEntityDestroySystem : DestroySystem<AssetEntity>
    {
        public override void Destroy(AssetEntity self)
        {
            if (PoolingAssetComponent.Instance.PathAssetEntityPools.TryGetValue(self.AssetPath, out var assetEntityPool))
            {
                assetEntityPool.RecycleGameObject(self.Object);
                self.Destroy();
            }
        }
    }
}