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


			var uiTypes = Game.EventSystem.GetTypes(typeof (UIPanelAttribute));
			foreach (var uiType in uiTypes)
			{
				object[] attrs = uiType.GetCustomAttributes(typeof(UIPanelAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				UIPanelAttribute uiPanelAttribute = attrs[0] as UIPanelAttribute;
				self.UITypes.Add(uiPanelAttribute.UIType, uiType);
			}
		}
	}
	
	/// <summary>
	/// 管理所有UI GameObject 以及UI事件
	/// </summary>
	public static class UIEventComponentSystem
	{
		public static async ETTask<UIPanel> OnCreate(this UIEventComponent self, UIPanelComponent uiPanelComponent, int uiType)
		{
			try
			{
				var uiconfig = UIPanelConfigCategory.Instance.Get(uiType);
				var UIAssetPathIndex = uiconfig.AssetPath;
				var uiLayer = uiconfig.UILayer;
				var assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(UIAssetPathIndex);
				if (!self.UITypes.TryGetValue(uiType, out var UIComponentType))
				{
					Log.Error($"UIType:{uiType.ToString()} 对应的component未找到！");
					return null;
				}
				UIPanel uiPanel = EntityFactory.CreateWithParent<UIPanel, int, AssetEntity>(uiPanelComponent, uiType, assetEntity);
				uiPanel.AddComponent(UIComponentType);
				uiPanel.UIPanelAssetEntity.Object.transform.SetParent(self.UILayers[uiLayer]);
				await EventSystem.Instance.EnableAsync(uiPanel.GetComponent(UIComponentType));  
				uiPanel.UIPanelAssetEntity.Object.GetComponent<Canvas>().enabled = true;
				return uiPanel;
			}
			catch (Exception e)
			{
				throw new Exception($"on create ui error: {uiType}", e);
			}
		}
		
		public static async ETTask<UIPanel> OnCreate<T>(this UIEventComponent self, UIPanelComponent uiPanelComponent, int uiType,T args)
		{
			try
			{
				var uiconfig = UIPanelConfigCategory.Instance.Get(uiType);
				var UIAssetPathIndex = uiconfig.AssetPath;
				var uiLayer = uiconfig.UILayer;
				var assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(UIAssetPathIndex);
				if (!self.UITypes.TryGetValue(uiType, out var UIComponentType))
				{
					Log.Error($"UIType:{uiType.ToString()} 对应的component未找到！");
					return null;
				}
				UIPanel uiPanel = EntityFactory.CreateWithParent<UIPanel, int, AssetEntity>(uiPanelComponent, uiType, assetEntity);
				uiPanel.AddComponent(UIComponentType);
				uiPanel.UIPanelAssetEntity.Object.transform.SetParent(self.UILayers[uiLayer]);
				await EventSystem.Instance.EnableAsync(uiPanel.GetComponent(UIComponentType),args);
				uiPanel.UIPanelAssetEntity.Object.GetComponent<Canvas>().enabled = true;
				return uiPanel;
			}
			catch (Exception e)
			{
				throw new Exception($"on create ui error: {uiType}", e);
			}
		}

		public static async ETTask<UIPanel> OnResume(this UIEventComponent self, UIPanel existUIPanel)
		{
			var uiLayer = UIPanelConfigCategory.Instance.Get(existUIPanel.UIPanelType).UILayer;
			existUIPanel.UIPanelAssetEntity.Object.transform.SetParent(self.UILayers[uiLayer]);
			await EventSystem.Instance.EnableAsync(existUIPanel.GetComponent(self.UITypes[existUIPanel.UIPanelType]));
			existUIPanel.UIPanelAssetEntity.Object.GetComponent<Canvas>().enabled = true;
			return existUIPanel;
		}
		
		public static async ETTask<UIPanel> OnResume<T>(this UIEventComponent self, UIPanel existUIPanel, T args)
		{
			var uiLayer = UIPanelConfigCategory.Instance.Get(existUIPanel.UIPanelType).UILayer;
			existUIPanel.UIPanelAssetEntity.Object.transform.SetParent(self.UILayers[uiLayer]);
			await EventSystem.Instance.EnableAsync(existUIPanel.GetComponent(self.UITypes[existUIPanel.UIPanelType]), args);
			existUIPanel.UIPanelAssetEntity.Object.GetComponent<Canvas>().enabled = true;
			return existUIPanel;
		}

		public static async ETTask OnPause(this UIEventComponent self, UIPanel existUIPanel)
		{
			if (existUIPanel!=null)
			{
				await EventSystem.Instance.DisableAsync(existUIPanel.GetComponent(self.UITypes[existUIPanel.UIPanelType]));
				existUIPanel.UIPanelAssetEntity.Object.GetComponent<Canvas>().enabled = false;
				existUIPanel.UIPanelAssetEntity.Object.transform.SetParent(self.UILayers[(int)UILayer.Hidden]);
			}
		}

		public static async ETTask OnRemove(this UIEventComponent self,UIPanel existUIPanel)
		{
			await EventSystem.Instance.DisableAsync(existUIPanel.GetComponent(self.UITypes[existUIPanel.UIPanelType]));
			existUIPanel.UIPanelAssetEntity.Object.GetComponent<Canvas>().enabled = false;
		}

	}
	
}