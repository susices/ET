using ET.EventType;

namespace ET
{
    public class AfterShowUI_RemoveTimeOutTimer : AEvent<AfterShowUI>
    {
        protected override async ETTask Run(AfterShowUI args)
        {
            args.UIBaseWindow.RemoveComponent<HideUITimeOutComponent>();
            await ETTask.CompletedTask;
        }
    }
}