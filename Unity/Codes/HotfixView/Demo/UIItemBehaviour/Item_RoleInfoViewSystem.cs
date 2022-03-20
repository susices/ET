
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class Scroll_Item_RoleInfoDestroySystem : DestroySystem<Scroll_Item_RoleInfo> 
	{
		public override void Destroy( Scroll_Item_RoleInfo self )
		{
			self.DestroyWidget();
		}
	}
}
