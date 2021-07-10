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
        public GameObject Object { private set; get; }

        private AssetEntityPool AssetEntityPool;

        public override void Dispose()
        {
            
            AssetEntityPool.RecycleGameObject(this.Object);
            this.Object = null;
            AssetEntityPool = null;
            base.Dispose();
        }

        public void Awake(AssetEntityPool assetEntityPool, Transform parent = null)
        {
            AssetEntityPool = assetEntityPool;
            this.Object = assetEntityPool.FetchGameObject(parent);
        }
    }
    
    public class AssetEntityAwakeSystem : AwakeSystem<AssetEntity,AssetEntityPool, Transform>
    {
        public override void Awake(AssetEntity self, AssetEntityPool uiPanelType, Transform parent)
        {
            self.Awake(uiPanelType, parent);
        }
    }
    
}