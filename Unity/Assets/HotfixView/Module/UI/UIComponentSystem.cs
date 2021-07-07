using UnityEngine;

namespace ET
{
	public class UIComponentAwakeSystem : AwakeSystem<UIComponent>
	{
		public override void Awake(UIComponent self)
		{
		}
	}
	
	/// <summary>
	/// 管理Scene上的UI
	/// </summary>
	public static class UIComponentSystem
	{
		public static async ETTask<UI> Create(this UIComponent self, int uiType)
		{
			var existUI = self.Get(uiType);
			if (existUI!=null)
			{
				return await self.Resume(existUI);
			}
			UI ui = await UIEventComponent.Instance.OnCreate(self, uiType);
			self.UIs.Add(uiType, ui);
			return ui;
		}

		private static async ETTask<UI> Resume(this UIComponent self, UI existUI)
		{
			return await UIEventComponent.Instance.OnResume(existUI);
		}
		
		public static async ETTask Pause(this UIComponent self, int uiType)
		{
			var existUI = self.Get(uiType);
			if (existUI!=null)
			{
				await UIEventComponent.Instance.OnPause(existUI);
			}
		}  

		public static async ETTask Remove(this UIComponent self, int uiType)
		{
			if (!self.UIs.TryGetValue(uiType, out UI existUI))
			{
				return;
			}
			await UIEventComponent.Instance.OnRemove(existUI);
			self.UIs.Remove(uiType);
			existUI.Dispose();
		}

		public static UI Get(this UIComponent self, int uiUype)
		{
			UI ui = null;
			self.UIs.TryGetValue(uiUype, out ui);
			return ui;
		}
	}
}