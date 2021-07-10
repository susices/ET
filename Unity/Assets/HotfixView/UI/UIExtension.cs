namespace ET
{
    public static class UIHelper
    {
        public static async ETTask<UIPanel> ShowUIPanel(this Scene self, int uiType)
        {
            return await self.GetComponent<UIComponent>().CreateUIPanel(uiType);
        }
        
        public static async ETTask<UIPanel> ShowUIPanel<T>(this Scene self, int uiType, T args)
        {
            return await self.GetComponent<UIComponent>().CreateUIPanel(uiType,args);
        }
        
        public static async ETTask RemoveUIPanel(this Scene self, int uiType)
        {
            await self.GetComponent<UIComponent>().RemoveUIPanel(uiType);
        }
        
        public static async ETTask HideUIPanel(this Scene self, int uiType)
        {   
            await self.GetComponent<UIComponent>().PauseUIPanel(uiType);
        }
    }
}