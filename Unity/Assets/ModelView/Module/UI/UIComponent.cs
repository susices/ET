using System.Collections.Generic;

namespace ET
{
	/// <summary>
	/// 管理Scene上的UI
	/// </summary>
	public class UIComponent: Entity
	{
		public Dictionary<int, UI> UIs = new Dictionary<int, UI>();
	}
}