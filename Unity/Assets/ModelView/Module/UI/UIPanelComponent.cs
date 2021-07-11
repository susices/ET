using System.Collections.Generic;

namespace ET
{
	/// <summary>
	/// 管理一组UIPanel的组件
	/// </summary>
	public class UIPanelComponent: Entity
	{
		public Dictionary<int, UIPanel> UIPanels = new Dictionary<int, UIPanel>();
	}
}