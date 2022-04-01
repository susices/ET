using ET.EventType;

namespace ET
{
    public class AfterHideUI_AddTimeOutTimer : AEvent<AfterHideUI>
    {
        protected override async ETTask Run(AfterHideUI args)
        {
            args.UIBaseWindow.AddComponent<HideUITimeOutComponent>();
            await ETTask.CompletedTask;
        }
    }
}