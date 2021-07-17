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
        /// 此Panel当前是否活跃
        /// </summary>
        public bool IsActive;

        /// <summary>
        /// 是否为子Panel
        /// </summary>
        public bool IsSubPanel;

        
    }
}