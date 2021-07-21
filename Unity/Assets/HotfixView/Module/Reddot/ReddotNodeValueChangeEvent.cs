using ET.EventType;

namespace ET
{
    /// <summary>
    /// 红点节点值改变事件处理
    /// </summary>
    public class ReddotNodeValueChangeEvent : AEvent<EventType.ReddotNodeValueChange>
    {
        protected override async ETTask Run(ReddotNodeValueChange a)
        {
            await ETTask.CompletedTask;
        }
    }
}