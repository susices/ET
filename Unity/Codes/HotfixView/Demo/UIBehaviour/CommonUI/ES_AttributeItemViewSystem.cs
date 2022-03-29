
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class ES_AttributeItemAwakeSystem : AwakeSystem<ES_AttributeItem,Transform> 
	{
		public override void Awake(ES_AttributeItem self,Transform transform)
		{
			self.uiTransform = transform;
		}
	}


	[ObjectSystem]
	public class ES_AttributeItemDestroySystem : DestroySystem<ES_AttributeItem> 
	{
		public override void Destroy(ES_AttributeItem self)
		{
			self.DestroyWidget();
		}
	}
}
