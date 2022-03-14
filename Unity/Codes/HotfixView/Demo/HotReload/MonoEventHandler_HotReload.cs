namespace ET
{
    public class MonoEventHandler_HotReload:AEvent<MonoEvent>
    {
        protected  override async ETTask Run(MonoEvent monoEvent)
        {
            switch (monoEvent.EventType)
            {
                case MonoEventType.HotReloadCode:
                    HotReloadHelper.ReloadCode();
                    break;
                case MonoEventType.HotReloadConfig:
                    ConfigComponent.Instance.Load();
                    break;
            }
            await ETTask.CompletedTask;
        }
    }
}