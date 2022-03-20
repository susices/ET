
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgServerListViewComponentAwakeSystem : AwakeSystem<DlgServerListViewComponent> 
	{
		public override void Awake(DlgServerListViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgServerListViewComponentDestroySystem : DestroySystem<DlgServerListViewComponent> 
	{
		public override void Destroy(DlgServerListViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
