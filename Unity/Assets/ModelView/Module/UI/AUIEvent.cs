namespace ET
{
    public abstract class AUIEvent
    {
        public abstract ETTask<UIPanel> OnCreate(UIPanelComponent uiPanelComponent);
        public abstract void OnRemove(UIPanelComponent uiPanelComponent);
    }
}