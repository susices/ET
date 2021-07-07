namespace ET
{
    public static class UIHelper
    {
        public static async ETTask<UI> ShowUI(Scene scene, int uiType)
        {
            return await scene.GetComponent<UIComponent>().Create(uiType);
        }
        
        public static async ETTask RemoveUI(Scene scene, int uiType)
        {
            await scene.GetComponent<UIComponent>().Remove(uiType);
            await ETTask.CompletedTask;
        }

        public static async ETTask HideUI(Scene scene, int uiType)
        {   
            await scene.GetComponent<UIComponent>().Pause(uiType);
            await ETTask.CompletedTask;
        }
    }
}