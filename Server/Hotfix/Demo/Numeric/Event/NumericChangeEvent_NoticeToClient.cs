using ET.EventType;

namespace ET
{
    public class NumericChangeEvent_NoticeToClient : AEvent<EventType.NumbericChange>
    {
        protected override async ETTask Run(NumbericChange args)
        {
            if (!(args.Parent is Unit unit))
            {
                return;
            }
            unit.GetComponent<NumericNoticeComponent>()?.NoticeImmediately(args);
            await ETTask.CompletedTask;
        }
    }
}