using UnityEngine;

namespace ET
{
    /// <summary>
    /// 资源实体
    /// </summary>
    public class AssetEntity : Entity
    {
        /// <summary>
        /// 实例化gameObject
        /// </summary>
        public GameObject GameObject { private set; get; }

        private AssetEntityPool AssetEntityPool;

        public override void Dispose()
        {
            AssetEntityPool.RecycleGameObject(GameObject);
            GameObject = null;
            AssetEntityPool = null;
            base.Dispose();
        }

        public void Awake(AssetEntityPool assetEntityPool)
        {
            AssetEntityPool = assetEntityPool;
            GameObject = assetEntityPool.FetchGameObject();
        }
    }
    
    public class AssetEntityAwakeSystem : AwakeSystem<AssetEntity,AssetEntityPool>
    {
        public override void Awake(AssetEntity self, AssetEntityPool assetEntityPool)
        {
            self.Awake(assetEntityPool);
        }
    }
}