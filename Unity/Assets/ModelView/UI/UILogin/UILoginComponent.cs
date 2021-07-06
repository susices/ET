using System;
using System.Net;

using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[UIType(UiType.UILogin)]
	public class UILoginComponent: Entity
	{
		/// <summary>
		/// 用户名Ipt
		/// </summary>
		public InputField accountIpt;

		/// <summary>
		/// 密码Ipt
		/// </summary>
		public InputField passwordIpt;
		
		/// <summary>
		/// 登录Btn
		/// </summary>
		public Button loginBtn;
		
		/// <summary>
		/// 注册Btn
		/// </summary>
		public Button registerBtn;
	}
}
