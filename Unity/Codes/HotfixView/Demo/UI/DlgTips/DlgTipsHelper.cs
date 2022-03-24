namespace ET
{
    public static class DlgTipsHelper
    {
        public static async ETTask<int> ShowTips(Scene zoneScene, string context, string confirmBtnText = null, string cancelBtnText = null)
        {
            ShowWindowData showWindowData = zoneScene.GetComponent<UIComponent>().AddChild<ShowWindowData>();
            UITipsData uiTipsData = showWindowData.AddChild<UITipsData>();
            showWindowData.contextData = uiTipsData;
            Log.Debug($"context: {context}");
            uiTipsData.ContextText = context;
            Log.Debug($"uiTipsData.ConfirmText xxxxx {uiTipsData.ContextText}");
            uiTipsData.ConfirmText = confirmBtnText ?? "确定";
            uiTipsData.CancelText = cancelBtnText ?? "取消";
            zoneScene.GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Tips, showData: showWindowData);
            DlgTips dlgTips = zoneScene.GetComponent<UIComponent>().GetDlgLogic<DlgTips>();
            if (dlgTips==null)
            {
                Log.Error("无法获取DlgTips页面");
                return TipsResultType.Error;
            }
            return await dlgTips.Result;
        }
    }
}