namespace ET
{
    public class UIPanelComponentAwakeSystem: AwakeSystem<UIPanelComponent>
    {
        public override void Awake(UIPanelComponent self)
        {
        }
    }

    /// <summary>
    /// 管理Scene上的UI
    /// </summary>
    public static class UIPanelComponentSystem
    {
        /// <summary>
        /// 显示UIPanel 无参数
        /// </summary>
        public static async ETTask<UIPanel> ShowUIPanel(this UIPanelComponent self, int uiPanelType, bool isSubPanel)
        {
            UIPanel existUI = self.Get(uiPanelType);
            if (existUI != null)
            {
                return await self.ResumeUIPanel(existUI);
            }

            UIPanel uiPanel = await UIEventComponent.Instance.OnCreateUIPanel(self, uiPanelType, isSubPanel);
            self.UIPanels.Add(uiPanelType, uiPanel);
            return uiPanel;
        }

        /// <summary>
        /// 显示UIPanel 泛型参数
        /// </summary>
        public static async ETTask<UIPanel> ShowUIPanel<T>(this UIPanelComponent self, int uiPanelType, bool isSubPanel, T args)
        {
            UIPanel existUI = self.Get(uiPanelType);
            if (existUI != null)
            {
                return await self.ResumeUIPanel(existUI, args);
            }

            UIPanel uiPanel = await UIEventComponent.Instance.OnCreateUIPanel(self, uiPanelType,isSubPanel, args);
            self.UIPanels.Add(uiPanelType, uiPanel);
            return uiPanel;
        }

        /// <summary>
        /// 恢复显示UIPanel 无参数
        /// </summary>
        private static async ETTask<UIPanel> ResumeUIPanel(this UIPanelComponent self, UIPanel existUIPanel)
        {
            return await UIEventComponent.Instance.OnResumeUIPanel(existUIPanel);
        }

        /// <summary>
        /// 恢复显示UIPanel 泛型参数
        /// </summary>
        private static async ETTask<UIPanel> ResumeUIPanel<T>(this UIPanelComponent self, UIPanel existUIPanel, T args)
        {
            return await UIEventComponent.Instance.OnResumeUIPanel(existUIPanel,args);
        }

        /// <summary>
        /// 隐藏UIPanel
        /// </summary>
        public static async ETTask HideUIPanel(this UIPanelComponent self, int uiPanelType)
        {
            UIPanel existUI = self.Get(uiPanelType);
            if (existUI != null)
            {
                await UIEventComponent.Instance.OnPauseUIPanel(existUI);
            }
            else
            {
                Log.Error($"uiPanelType：{uiPanelType.ToString()} 不存在此UIPanelComponent中");
            }
        }

        /// <summary>
        /// 移除UIPanel
        /// </summary>
        public static async ETTask RemoveUIPanel(this UIPanelComponent self, int uiPanelType)
        {
            if (!self.UIPanels.TryGetValue(uiPanelType, out UIPanel existUI))
            {
                Log.Error($"uiPanelType：{uiPanelType.ToString()} 不存在此UIPanelComponent中");
                return;
            }

            await UIEventComponent.Instance.OnRemoveUIPanel(existUI);
            self.UIPanels.Remove(uiPanelType);
            existUI.Dispose();
        }

        /// <summary>
        /// 获取已有UIPanel
        /// </summary>
        public static UIPanel Get(this UIPanelComponent self, int uiPanelType)
        {
            self.UIPanels.TryGetValue(uiPanelType, out UIPanel uiPanel);
            return uiPanel;
        }
    }
}