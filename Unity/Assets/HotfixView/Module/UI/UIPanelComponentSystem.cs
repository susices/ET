using UnityEngine;
namespace ET
{
	public class UIPanelComponentAwakeSystem : AwakeSystem<UIPanelComponent>
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
		public static async ETTask<UIPanel> CreateUIPanel(this UIPanelComponent self, int uiPanelType)
		{
			var existUI = self.Get(uiPanelType);
			if (existUI!=null)
			{
				return await self.ResumeUIPanel(existUI);
			}
			UIPanel uiPanel = await UIEventComponent.Instance.OnCreate(self, uiPanelType);
			self.UIPanels.Add(uiPanelType, uiPanel);
			return uiPanel;
		}
		
		public static async ETTask<UIPanel> CreateUIPanel<T>(this UIPanelComponent self, int uiPanelType, T args)
		{
			var existUI = self.Get(uiPanelType);
			if (existUI!=null)
			{
				return await self.ResumeUIPanel(existUI,args);
			}
			UIPanel uiPanel = await UIEventComponent.Instance.OnCreate(self, uiPanelType,args);
			self.UIPanels.Add(uiPanelType, uiPanel);
			return uiPanel;
		}

		private static async ETTask<UIPanel> ResumeUIPanel(this UIPanelComponent self, UIPanel existUIPanel)
		{
			return await UIEventComponent.Instance.OnResume(existUIPanel);
		}
		
		private static async ETTask<UIPanel> ResumeUIPanel<T>(this UIPanelComponent self, UIPanel existUIPanel, T args)
		{
			return await UIEventComponent.Instance.OnResume(existUIPanel,args);
		}

		public static async ETTask PauseUIPanel(this UIPanelComponent self, int uiPanelType)
		{
			var existUI = self.Get(uiPanelType);
			if (existUI!=null)
			{
				await UIEventComponent.Instance.OnPause(existUI);
			}
		}  

		public static async ETTask RemoveUIPanel(this UIPanelComponent self, int uiPanelType)
		{
			if (!self.UIPanels.TryGetValue(uiPanelType, out UIPanel existUI))
			{
				return;
			}
			await UIEventComponent.Instance.OnRemove(existUI);
			self.UIPanels.Remove(uiPanelType);
			existUI.Dispose();
		}

		public static UIPanel Get(this UIPanelComponent self, int uiPanelType)
		{
			UIPanel uiPanel = null;
			self.UIPanels.TryGetValue(uiPanelType, out uiPanel);
			return uiPanel;
		}
	}
}