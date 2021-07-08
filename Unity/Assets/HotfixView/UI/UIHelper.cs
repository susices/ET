namespace ET
{
    public static class UIHelper
    {
        public static async ETTask<UI> ShowUIPanel(Scene scene, int uiType)
        {
            return await scene.GetComponent<UIComponent>().CreateUIPanel(uiType);
        }
        
        public static async ETTask RemoveUIPanel(Scene scene, int uiType)
        {
            await scene.GetComponent<UIComponent>().RemoveUIPanel(uiType);
            await ETTask.CompletedTask;
        }

        public static async ETTask HideUIPanel(Scene scene, int uiType)
        {   
            await scene.GetComponent<UIComponent>().PauseUIPanel(uiType);
            await ETTask.CompletedTask;
        }
    }
}