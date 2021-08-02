namespace ET
{
    public class EnterMapFinish_AddUIHUD:AEvent<EventType.EnterMapFinish>
    {
        protected override async ETTask Run(EventType.EnterMapFinish args)
        {
            await args.ZoneScene.ShowUIPanel(UIPanelType.UIHUD);
        }
    }
}