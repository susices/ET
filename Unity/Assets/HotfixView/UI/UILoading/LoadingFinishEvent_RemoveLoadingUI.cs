namespace ET
{
    public class LoadingFinishEvent_RemoveLoadingUI : AEvent<EventType.LoadingFinish>
    {
        protected override async ETTask Run(EventType.LoadingFinish args)
        {
            await UIHelper.Remove(args.Scene, UiType.UILodding);
        }
    }
}
