using ET.EventType;

namespace ET
{
    public class ReddotNodeNumChangeEvent : AEvent<EventType.ReddotNodeNumChange>
    {
        protected override async ETTask Run(ReddotNodeNumChange a)
        {
            await ETTask.CompletedTask;
        }
    }
}