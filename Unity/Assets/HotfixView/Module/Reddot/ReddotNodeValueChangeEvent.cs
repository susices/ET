using ET.EventType;

namespace ET
{
    /// <summary>
    /// 红点节点值改变事件处理
    /// </summary>
    public class ReddotNodeValueChangeEvent : AEvent<EventType.ReddotNodeValueChange>
    {
        protected override async ETTask Run(ReddotNodeValueChange reddotNodeValueChange)
        {
            var redDotNodeEntity = RedDotManagerComponent.Instance.GetReddotNode(reddotNodeValueChange.ReddotNodePath);
            if (redDotNodeEntity==null)
            {
                return;
            }

            foreach (var redDotUI in redDotNodeEntity.RedDotUIEntities.Values)
            {
                if (redDotUI is RedDotUIEntity redDotUIComponent)
                {
                    redDotUIComponent.OnNodeValueChange(reddotNodeValueChange.NewValue);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}