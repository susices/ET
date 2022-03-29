using ET.EventType;

namespace ET
{
    [NumericWatcher(NumericType.Level)]
    [NumericWatcher(NumericType.Exp)]
    [NumericWatcher(NumericType.Gold)]
    public class NumericWatcher_RefreshMainUI: INumericWatcher
    {
        public void Run(NumbericChange args)
        {
            args.Parent.ZoneScene()?.GetComponent<UIComponent>()?.GetDlgLogic<DlgMain>()?.Refresh();
        }
    }
}