using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public static class DlgTipsSystem
	{

		public static void RegisterUIEvent(this DlgTips self)
		{
			self.View.EButton_ConfirmButton.AddListener(() =>
			{
				self.OnClickBtnHandler(TipsResultType.Confirm);
			});
			
			self.View.EButton_CancelButton.AddListener(() =>
			{
				self.OnClickBtnHandler(TipsResultType.Cancel);
			});
		}

		public static void ShowWindow(this DlgTips self, Entity contextData = null)
		{
			self.Result = ETTask<int>.Create();
			if (contextData != null && contextData is UITipsData uiTipsData)
			{
				self.View.ELabel_TipsTextText.text = uiTipsData.ContextText;
				self.View.E_ConfirmBtnText.text = uiTipsData.ConfirmText;
				self.View.E_CancelBtnText.text = uiTipsData.CancelText;
				uiTipsData.GetParent<ShowWindowData>().Dispose();
			}
			else
			{
				Log.Error("contextData is null");
			}
		}

		public static void HideWindow(this DlgTips self)
		{
			self.Result = null;
		}

		public static void OnClickBtnHandler(this DlgTips self, int tipsButtonType)
		{
			self.Result.SetResult(tipsButtonType);
			self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Tips);
		}
	}
}
