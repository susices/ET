namespace ET
{
    public class AfterCreateCurrentScene_AddComponent: AEvent<EventType.AfterCreateCurrentScene>
    {
        protected override async ETTask Run(EventType.AfterCreateCurrentScene args)
        {
            Scene currentScene = args.CurrentScene;
            currentScene.AddComponent<UIComponent>();
            await ETTask.CompletedTask;
        }
    }
}