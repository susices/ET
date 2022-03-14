namespace ET
{
    public class MonoEventHandler_HotReload:AEvent<MonoEvent>
    {
        protected  override async ETTask Run(MonoEvent monoEvent)
        {
            if (monoEvent.EventType == MonoEventType.HotReloadCode)
            {
                HotReloadHelper.ReloadCode();
            }
            await ETTask.CompletedTask;
        }
    }
}