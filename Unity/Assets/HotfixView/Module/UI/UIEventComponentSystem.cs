using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	public class UIEventComponentAwakeSystem : AwakeSystem<UIEventComponent>
	{
		public override void Awake(UIEventComponent self)
		{
			UIEventComponent.Instance = self;
			
			GameObject uiRoot = GameObject.Find("/Global/UI");
			ReferenceCollector referenceCollector = uiRoot.GetComponent<ReferenceCollector>();
			
			self.UILayers.Add((int)UILayer.Hidden, referenceCollector.Get<GameObject>(UILayer.Hidden.ToString()).transform);
			self.UILayers.Add((int)UILayer.Low, referenceCollector.Get<GameObject>(UILayer.Low.ToString()).transform);
			self.UILayers.Add((int)UILayer.Mid, referenceCollector.Get<GameObject>(UILayer.Mid.ToString()).transform);
			self.UILayers.Add((int)UILayer.High, referenceCollector.Get<GameObject>(UILayer.High.ToString()).transform);

			var uiEvents = Game.EventSystem.GetTypes(typeof (UIEventAttribute));
			foreach (Type type in uiEvents)
			{
				object[] attrs = type.GetCustomAttributes(typeof(UIEventAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				UIEventAttribute uiEventAttribute = attrs[0] as UIEventAttribute;
				AUIEvent aUIEvent = Activator.CreateInstance(type) as AUIEvent;
				self.UIEvents.Add(uiEventAttribute.UIType, aUIEvent);
			}


			var uiTypes = Game.EventSystem.GetTypes(typeof (UITypeAttribute));
			foreach (var uiType in uiTypes)
			{
				object[] attrs = uiType.GetCustomAttributes(typeof(UITypeAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				UITypeAttribute uiTypeAttribute = attrs[0] as UITypeAttribute;
				self.UITypes.Add(uiTypeAttribute.UIType, uiType);
			}
		}
	}
	
	/// <summary>
	/// 管理所有UI GameObject 以及UI事件
	/// </summary>
	public static class UIEventComponentSystem
	{
		public static async ETTask<UI> OnCreate(this UIEventComponent self, UIComponent uiComponent, int uiType)
		{
			try
			{
				var uiconfig = UIConfigCategory.Instance.Get(uiType);
				var UIAssetPath = uiconfig.AssetPath;
				var uiLayer = uiconfig.UILayer;
				var assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(UIAssetPath);
				if (!self.UITypes.TryGetValue(uiType, out var UIComponentType))
				{
					Log.Error($"UIType:{uiType.ToString()} 对应的component未找到！");
					return null;
				}
				UI ui = EntityFactory.CreateWithParent<UI, int, AssetEntity,Type>(uiComponent, uiType, assetEntity,UIComponentType);
				ui.AddComponent(UIComponentType);
				ui.UIAssetEntity.GameObject.transform.SetParent(self.UILayers[uiLayer]);
				return ui;
			}
			catch (Exception e)
			{
				throw new Exception($"on create ui error: {uiType}", e);
			}
		}
		
	}
}