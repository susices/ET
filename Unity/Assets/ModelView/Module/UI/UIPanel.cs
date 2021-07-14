namespace ET
{
    public sealed class UIPanel: Entity
    {
        /// <summary>
        /// UIPanel资源实体
        /// </summary>
        public AssetEntity UIPanelAssetEntity;
        
        public int UIPanelType;

        /// <summary>
        /// 是否为子Panel
        /// </summary>
        public bool IsSubPanel;
    }
}