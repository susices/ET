
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgTipsViewComponentAwakeSystem : AwakeSystem<DlgTipsViewComponent> 
	{
		public override void Awake(DlgTipsViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgTipsViewComponentDestroySystem : DestroySystem<DlgTipsViewComponent> 
	{
		public override void Destroy(DlgTipsViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
