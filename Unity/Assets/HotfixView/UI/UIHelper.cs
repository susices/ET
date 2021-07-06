namespace ET
{
    public static class UIHelper
    {
        public static async ETTask<UI> Show(Scene scene, int uiType)
        {
            return await scene.GetComponent<UIComponent>().Show(uiType);
        }
        
        public static async ETTask Remove(Scene scene, int uiType)
        {
            scene.GetComponent<UIComponent>().Remove(uiType);
            await ETTask.CompletedTask;
        }

        public static async ETTask Hide(Scene scene, int uiType)
        {   
            scene.GetComponent<UIComponent>().Hide(uiType);
            await ETTask.CompletedTask;
        }
    }
}