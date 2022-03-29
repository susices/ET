
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgRoleInfoViewComponentAwakeSystem : AwakeSystem<DlgRoleInfoViewComponent> 
	{
		public override void Awake(DlgRoleInfoViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgRoleInfoViewComponentDestroySystem : DestroySystem<DlgRoleInfoViewComponent> 
	{
		public override void Destroy(DlgRoleInfoViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
