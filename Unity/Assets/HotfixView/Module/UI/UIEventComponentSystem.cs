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
			
			var uiPanelTypes = Game.EventSystem.GetTypes(typeof (UIPanelAttribute));
			foreach (var uiType in uiPanelTypes)
			{
				object[] attrs = uiType.GetCustomAttributes(typeof(UIPanelAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				UIPanelAttribute uiPanelAttribute = attrs[0] as UIPanelAttribute;
				self.UIPanelTypes.Add(uiPanelAttribute.UIPanelType, uiType);
			}

			var uiItemTypes = Game.EventSystem.GetTypes(typeof (UIItemAttribute));
			foreach (var uiItemType in uiItemTypes)
			{
				object[] attrs = uiItemType.GetCustomAttributes(typeof (UIItemAttribute), false);
				if (attrs.Length ==0 )
				{
					continue;
				}
				UIItemAttribute uiItemAttribute = attrs[0] as UIItemAttribute;
				self.UIItemTypes.Add(uiItemAttribute.UIItemType, uiItemType);
			}
		}
	}
	
	/// <summary>
	/// 管理所有UI GameObject 以及UI事件
	/// </summary>
	public static class UIEventComponentSystem
	{
		public static async ETTask<UIPanel> OnCreateUIPanel(this UIEventComponent self, UIPanelComponent uiPanelComponent, int uiType, bool isSubPanel)
		{
			try
			{
				if (!self.UIPanelTypes.TryGetValue(uiType, out var UIComponentType))
				{
					Log.Error($"UIType:{uiType.ToString()} 对应的UIPanelComponent未找到！");
					return null;
				}
				var uiconfig = UIPanelConfigCategory.Instance.Get(uiType);
				var UIAssetPathIndex = uiconfig.AssetPath;
				var assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(UIAssetPathIndex);
				
				UIPanel uiPanel = EntityFactory.CreateWithParent<UIPanel, int, AssetEntity, bool>(uiPanelComponent, uiType, assetEntity, isSubPanel, true);
				uiPanel.AddComponent(UIComponentType);
				UIHelper.SetUIPanelParent(uiPanel);
				await EventSystem.Instance.EnableAsync(uiPanel.GetComponent(UIComponentType));
				if (uiPanel.UIPanelAssetEntity.Object.TryGetComponent<Canvas>(out var canvas))
				{
					canvas.enabled = true;
				}
				return uiPanel;
			}
			catch (Exception e)
			{
				throw new Exception($"on create ui error: {uiType}", e);
			}
		}
		
		public static async ETTask<UIPanel> OnCreateUIPanel<T>(this UIEventComponent self, UIPanelComponent uiPanelComponent, int uiType, bool isSubPanel,T args)
		{
			try
			{
				if (!self.UIPanelTypes.TryGetValue(uiType, out var UIComponentType))
				{
					Log.Error($"UIType:{uiType.ToString()} 对应的UIPanelComponent未找到！");
					return null;
				}
				var uiconfig = UIPanelConfigCategory.Instance.Get(uiType);
				var UIAssetPathIndex = uiconfig.AssetPath;
				var assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(UIAssetPathIndex);
				
				UIPanel uiPanel = EntityFactory.CreateWithParent<UIPanel, int, AssetEntity, bool>(uiPanelComponent, uiType, assetEntity, isSubPanel, true);
				uiPanel.AddComponent(UIComponentType); 
				UIHelper.SetUIPanelParent(uiPanel);
				await EventSystem.Instance.EnableAsync(uiPanel.GetComponent(UIComponentType),args);
				if (uiPanel.UIPanelAssetEntity.Object.TryGetComponent<Canvas>(out var canvas))
				{
					canvas.enabled = true;
				}
				return uiPanel;
			}
			catch (Exception e)
			{
				throw new Exception($"on create ui error: {uiType}", e);
			}
		}

		public static async ETTask<UIPanel> OnResumeUIPanel(this UIEventComponent self, UIPanel existUIPanel)
		{
			UIHelper.SetUIPanelParent(existUIPanel);
			await EventSystem.Instance.EnableAsync(existUIPanel.GetComponent(self.UIPanelTypes[existUIPanel.UIPanelType]));
			if (existUIPanel.UIPanelAssetEntity.Object.TryGetComponent<Canvas>(out var canvas))
			{
				canvas.enabled = true;
			}
			return existUIPanel;
		}
		
		public static async ETTask<UIPanel> OnResumeUIPanel<T>(this UIEventComponent self, UIPanel existUIPanel, T args)
		{
			UIHelper.SetUIPanelParent(existUIPanel);
			await EventSystem.Instance.EnableAsync(existUIPanel.GetComponent(self.UIPanelTypes[existUIPanel.UIPanelType]), args);
			if (existUIPanel.UIPanelAssetEntity.Object.TryGetComponent<Canvas>(out var canvas))
			{
				canvas.enabled = true;
			}
			return existUIPanel;
		}

		public static async ETTask OnPauseUIPanel(this UIEventComponent self, UIPanel existUIPanel)
		{
			if (existUIPanel!=null)
			{
				await EventSystem.Instance.DisableAsync(existUIPanel.GetComponent(self.UIPanelTypes[existUIPanel.UIPanelType]));
				if (existUIPanel.UIPanelAssetEntity.Object.TryGetComponent<Canvas>(out var canvas))
				{
					canvas.enabled = false;
				}
				existUIPanel.UIPanelAssetEntity.Object.transform.SetParent(self.UILayers[(int)UILayer.Hidden]);
			}
		}

		public static async ETTask OnRemoveUIPanel(this UIEventComponent self,UIPanel existUIPanel)
		{
			await EventSystem.Instance.DisableAsync(existUIPanel.GetComponent(self.UIPanelTypes[existUIPanel.UIPanelType]));
			if (existUIPanel.UIPanelAssetEntity.Object.TryGetComponent<Canvas>(out var canvas))
			{
				canvas.enabled = false;
			}
		}

		public static async ETTask<UIItem> OnCreateUIItem(this UIEventComponent self, UIPanel uiPanel, int uiItemType, Transform parentTransform)
		{
			if (!self.UIItemTypes.TryGetValue(uiItemType, out Type uiItemComponentType))
			{
				Log.Error($"UIType:{uiItemType.ToString()} 对应的UIItemComponent未找到！");
				return null;
			}
			UIItemConfig uiItemConfig = UIItemConfigCategory.Instance.Get(uiItemType);
			AssetEntity assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiItemConfig.AssetPath, parentTransform);
			UIItem uiItem = EntityFactory.CreateWithParent<UIItem, int, AssetEntity>(uiPanel, uiItemType, assetEntity);
			uiItem.AddComponent(uiItemComponentType);
			await EventSystem.Instance.EnableAsync(uiItem.GetComponent(uiItemComponentType));
			if (uiItem.UIItemAssetEntity.Object.TryGetComponent<CanvasGroup>(out var canvasGroup))
			{
				canvasGroup.alpha = 1;
			}
			return uiItem;
		}
		
		public static async ETTask<UIItem> OnCreateUIItem<T>(this UIEventComponent self, UIPanel uiPanel, int uiItemType,T args,Transform parentTransform)
		{
			if (!self.UIItemTypes.TryGetValue(uiItemType, out Type uiItemComponentType))
			{
				Log.Error($"UIType:{uiItemType.ToString()} 对应的UIItemComponent未找到！");
				return null;
			}
			UIItemConfig uiItemConfig = UIItemConfigCategory.Instance.Get(uiItemType);
			AssetEntity assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiItemConfig.AssetPath, parentTransform);
			UIItem uiItem = EntityFactory.CreateWithParent<UIItem, int, AssetEntity>(uiPanel, uiItemType, assetEntity);
			uiItem.AddComponent(uiItemComponentType);
			await EventSystem.Instance.EnableAsync(uiItem.GetComponent(uiItemComponentType),args);
			if (uiItem.UIItemAssetEntity.Object.TryGetComponent<CanvasGroup>(out var canvasGroup))
			{
				canvasGroup.alpha = 1;
			}
			return uiItem;
		}
	}
	
}