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
		public static async ETTask<UI> Show(this UIComponent self, int uiType)
		{
			var existUI = self.Get(uiType);
			if (existUI!=null)
			{
				return await self.ReShow(existUI);
			}
			UI ui = await UIEventComponent.Instance.OnCreate(self, uiType);
			self.UIs.Add(uiType, ui);
			return ui;
		}

		private static async ETTask<UI> ReShow(this UIComponent self, UI existUI)
		{
			existUI.UIAssetEntity.GameObject.GetComponent<Canvas>().enabled = true;
			await ETTask.CompletedTask;
			return null;
		}
		
		public static void Hide(this UIComponent self, int uiType)
		{
			var existUI = self.Get(uiType);
			if (existUI!=null)
			{
				existUI.UIAssetEntity.GameObject.GetComponent<Canvas>().enabled = false;
			}
		}
		
		

		public static void Remove(this UIComponent self, int uiType)
		{
			if (!self.UIs.TryGetValue(uiType, out UI ui))
			{
				return;
			}
			self.UIs.Remove(uiType);
			ui.Dispose();
		}

		

		public static UI Get(this UIComponent self, int uiUype)
		{
			UI ui = null;
			self.UIs.TryGetValue(uiUype, out ui);
			return ui;
		}
	}
}