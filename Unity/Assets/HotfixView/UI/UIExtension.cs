namespace ET
{
    public static class UIExtension
    {
        public static async ETTask<UIPanel> ShowUIPanel(this Scene self, int uiPanelType)
        {
            return await self.GetComponent<UIPanelComponent>().CreateUIPanel(uiPanelType);
        }
        
        public static async ETTask<UIPanel> ShowUIPanel<T>(this Scene self, int uiPanelType, T args)
        {
            return await self.GetComponent<UIPanelComponent>().CreateUIPanel(uiPanelType,args);
        }
        
        public static async ETTask RemoveUIPanel(this Scene self, int uiPanelType)
        {
            await self.GetComponent<UIPanelComponent>().RemoveUIPanel(uiPanelType);
        }
        
        public static async ETTask HideUIPanel(this Scene self, int uiPanelType)
        {   
            await self.GetComponent<UIPanelComponent>().PauseUIPanel(uiPanelType);
        }
    }
}