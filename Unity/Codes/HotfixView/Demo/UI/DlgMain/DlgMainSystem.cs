using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public static  class DlgMainSystem
	{

		public static void RegisterUIEvent(this DlgMain self)
		{
			self.View.E_RoleButton.AddListenerAsync(() => { return self.OnRoleBtnClickHandler();});
		}

		public static void ShowWindow(this DlgMain self, Entity contextData = null)
		{
			self.Refresh().Coroutine();
		}

		public static async ETTask Refresh(this DlgMain self)
		{
			Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene());
			NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
			self.View.E_RoleLevelText.SetText(numericComponent.GetAsInt(NumericType.Level).ToString());
			self.View.E_ExpText.SetText(numericComponent.GetAsInt(NumericType.Exp).ToString());
			self.View.E_GoldText.SetText(numericComponent.GetAsInt(NumericType.Gold).ToString());
			await ETTask.CompletedTask;
		}

		public static async ETTask OnRoleBtnClickHandler(this DlgMain self)
		{
			await self.ZoneScene().GetComponent<UIComponent>().HideAndShowWindowStackAsync(WindowID.WindowID_Main, WindowID.WindowID_RoleInfo);
		}

	}
}
