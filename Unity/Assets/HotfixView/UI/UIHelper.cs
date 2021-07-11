namespace ET
{
    
    public static class UIHelper
    {
        /// <summary>
        /// 设置UIPanel所属GameObject的父节点
        /// </summary>
        /// <param name="uiPanel"></param>
        public static void SetUIPanelParent(UIPanel uiPanel)
        {
            if (uiPanel.IsSubPanel)
            {
                var parentUIPanel = uiPanel.GetParent<UIPanelComponent>().GetParent<UIPanel>();
                uiPanel.UIPanelAssetEntity.Object.transform.SetParent(parentUIPanel.UIPanelAssetEntity.Object.transform);
            }
            else
            {
                var uiLayer = UIPanelConfigCategory.Instance.Get(uiPanel.UIPanelType).UILayer;
                uiPanel.UIPanelAssetEntity.Object.transform.SetParent(UIEventComponent.Instance.UILayers[uiLayer]);
            }
        }
    }
}