using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ET
{
	/// <summary>
	/// 管理所有UI GameObject
	/// </summary>
	public class UIEventComponent: Entity
	{
		public static UIEventComponent Instance;
		
		public Dictionary<string, AUIEvent> UIEvents = new Dictionary<string, AUIEvent>();
		
		public Dictionary<int, Transform> UILayers = new Dictionary<int, Transform>();

		public Dictionary<int, Type> UIPanelComponentTypes = new Dictionary<int, Type>();

		public Dictionary<int, Type> UIItemComponentTypes = new Dictionary<int, Type>();
	}
}