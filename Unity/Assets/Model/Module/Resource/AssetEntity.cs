using UnityEngine;

namespace ET
{
    /// <summary>
    /// 资源实体
    /// </summary>
    public class AssetEntity : Entity
    {
        public GameObject GameObject;

        public AssetEntityPool AssetEntityPool;

    }
    
    /// <summary>
    /// 资源实体系统
    /// </summary>
    public class AssetEntitySystem
    {
        
    }
    
    public class AssetEntityAwakeSystem : AwakeSystem<AssetEntity,AssetEntityPool>
    {
        public override void Awake(AssetEntity self, AssetEntityPool assetEntityPool)
        {
            self.AssetEntityPool = assetEntityPool;
            self.GameObject = assetEntityPool.FetchGameObject();
        }
    }

    public class AssetEntityDestroySystem : DestroySystem<AssetEntity>
    {
        public override void Destroy(AssetEntity self)
        {
            self.AssetEntityPool.RecycleGameObject(self.GameObject);
            self.GameObject = null;
            self.AssetEntityPool = null;
        }
    }
}