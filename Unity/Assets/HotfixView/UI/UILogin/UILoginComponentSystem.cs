using System;
using System.Net;

using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class UILoginComponentAwakeSystem : AwakeSystem<UILoginComponent>
	{
		public override void Awake(UILoginComponent self)
		{
			ReferenceCollector rc = self.GetParent<UIPanel>().UIPanelAssetEntity.Object.GetComponent<ReferenceCollector>();
			self.accountIpt = rc.Get<GameObject>("Account").GetComponent<InputField>();
			self.passwordIpt = rc.Get<GameObject>("Password").GetComponent<InputField>();
			self.loginBtn = rc.Get<GameObject>("LoginBtn").GetComponent<Button>();
			self.loginBtn.onClick.AddListener(self.OnLogin);
			self.registerBtn = rc.Get<GameObject>("RegisterBtn").GetComponent<Button>();
			self.registerBtn.onClick.AddListener(self.OnRegieter);
			Log.Debug("LoginAwake");
		}
	}
	
	public static class UILoginComponentSystem
	{
		public static void OnLogin(this UILoginComponent self)
		{
			LoginHelper.Login(self.DomainScene(), "127.0.0.1:10002", self.accountIpt.text,self.passwordIpt.text).Coroutine();
		}

		public static void OnRegieter(this UILoginComponent self)
		{
			LoginHelper.Register(self.DomainScene(), "127.0.0.1:10002", self.accountIpt.text, self.passwordIpt.text).Coroutine();
		}
	}
	
	public class UILoginComponentEnableSystem : EnableSystem<UILoginComponent>
	{
		public override async ETTask Enable(UILoginComponent self)
		{
			Log.Debug("Login Enable!");
			await ETTask.CompletedTask;
		}
	}
	
	public class UILoginComponentEnableSystemArgs : EnableSystem<UILoginComponent, string>
	{
		public override async ETTask Enable(UILoginComponent self, string args)
		{
			Log.Error(args);
			await ETTask.CompletedTask;
		}
	}

	public class UILoginComponentDisableSystem : DisableSystem<UILoginComponent>
	{
		public override async ETTask Disable(UILoginComponent self)
		{
			Log.Debug("Login Disable!");
			await ETTask.CompletedTask;
		}
	}
}
