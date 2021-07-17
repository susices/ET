using System;
using UnityEngine;

namespace ET
{
    public class UIEventComponentAwakeSystem: AwakeSystem<UIEventComponent>
    {
        public override void Awake(UIEventComponent self)
        {
            UIEventComponent.Instance = self;

            GameObject uiRoot = GameObject.Find("/Global/UI");
            ReferenceCollector referenceCollector = uiRoot.GetComponent<ReferenceCollector>();
            self.UILayers.Add((int) UILayer.Hidden, referenceCollector.Get<GameObject>(UILayer.Hidden.ToString()).transform);
            self.UILayers.Add((int) UILayer.Low, referenceCollector.Get<GameObject>(UILayer.Low.ToString()).transform);
            self.UILayers.Add((int) UILayer.Mid, referenceCollector.Get<GameObject>(UILayer.Mid.ToString()).transform);
            self.UILayers.Add((int) UILayer.High, referenceCollector.Get<GameObject>(UILayer.High.ToString()).transform);
            var uiEvents = Game.EventSystem.GetTypes(typeof (UIEventAttribute));
            foreach (Type type in uiEvents)
            {
                object[] attrs = type.GetCustomAttributes(typeof (UIEventAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                UIEventAttribute uiEventAttribute = attrs[0] as UIEventAttribute;
                AUIEvent aUIEvent = Activator.CreateInstance(type) as AUIEvent;
                if (uiEventAttribute != null)
                {
                    self.UIEvents.Add(uiEventAttribute.UIType, aUIEvent);
                }
            }

            var uiPanelTypes = Game.EventSystem.GetTypes(typeof (UIPanelComponentAttribute));
            foreach (Type uiType in uiPanelTypes)
            {
                object[] attrs = uiType.GetCustomAttributes(typeof (UIPanelComponentAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                if (attrs[0] is UIPanelComponentAttribute uiPanelComponentAttribute)
                {
                    self.UIPanelComponentTypes.Add(uiPanelComponentAttribute.ComponentIndex, uiType);
                }
            }

            var uiItemTypes = Game.EventSystem.GetTypes(typeof (UIItemAttribute));
            foreach (Type uiItemType in uiItemTypes)
            {
                object[] attrs = uiItemType.GetCustomAttributes(typeof (UIItemAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                if (attrs[0] is UIItemAttribute uiItemAttribute)
                {
                    self.UIItemComponentTypes.Add(uiItemAttribute.UIItemType, uiItemType);
                }
            }
        }
    }

    /// <summary>
    /// 管理所有UI GameObject 以及UI事件
    /// </summary>
    public static class UIEventComponentSystem
    {
        public static async ETTask<UIPanel> OnCreateUIPanel(this UIEventComponent self, UIPanelComponent uiPanelComponent, int uiType,
        bool isSubPanel)
        {
            try
            {
                using var componentTypeList = ListComponent<Type>.Create();
                UIHelper.GetUIPanelComponentTypeList(uiType, componentTypeList.List);
                if (componentTypeList.List.Count == 0)
                {
                    return null;
                }

                UIPanelConfig uiConfig = UIPanelConfigCategory.Instance.Get(uiType);
                int uiAssetPathIndex = uiConfig.AssetPath;
                AssetEntity assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiAssetPathIndex);
                UIPanel uiPanel =
                        EntityFactory.CreateWithParent<UIPanel, int, AssetEntity, bool>(uiPanelComponent, uiType, assetEntity, isSubPanel, true);
                UIHelper.SetUIPanelParent(uiPanel);

                foreach (Type uiComponentTye in componentTypeList.List)
                {
                    uiPanel.AddComponent(uiComponentTye);
                }

                await UIHelper.SetUIPanelEnableAsync(uiPanel, componentTypeList.List);

                if (uiPanel.UIPanelAssetEntity.Object.TryGetComponent(out Canvas canvas))
                {
                    canvas.enabled = true;
                }

                uiPanel.IsActive = true;
                return uiPanel;
            }
            catch (Exception e)
            {
                throw new Exception($"on create ui error: {uiType}", e);
            }
        }

        public static async ETTask<UIPanel> OnCreateUIPanel<T>(this UIEventComponent self, UIPanelComponent uiPanelComponent, int uiType,
        bool isSubPanel, T args)
        {
            try
            {
                using var componentTypeList = ListComponent<Type>.Create();
                UIHelper.GetUIPanelComponentTypeList(uiType, componentTypeList.List);
                if (componentTypeList.List.Count == 0)
                {
                    return null;
                }

                UIPanelConfig uiConfig = UIPanelConfigCategory.Instance.Get(uiType);
                int uiAssetPathIndex = uiConfig.AssetPath;
                AssetEntity assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiAssetPathIndex);
                UIPanel uiPanel =
                        EntityFactory.CreateWithParent<UIPanel, int, AssetEntity, bool>(uiPanelComponent, uiType, assetEntity, isSubPanel, true);
                UIHelper.SetUIPanelParent(uiPanel);

                foreach (Type uiComponentTye in componentTypeList.List)
                {
                    uiPanel.AddComponent(uiComponentTye);
                }

                await UIHelper.SetUIPanelEnableAsync(uiPanel, componentTypeList.List, args);
                if (uiPanel.UIPanelAssetEntity.Object.TryGetComponent(out Canvas canvas))
                {
                    canvas.enabled = true;
                }

                uiPanel.IsActive = true;
                return uiPanel;
            }
            catch (Exception e)
            {
                throw new Exception($"on create ui error: {uiType}", e);
            }
        }

        public static async ETTask<UIPanel> OnResumeUIPanel(this UIEventComponent self, UIPanel existUIPanel)
        {
            if (existUIPanel.IsActive)
            {
                return existUIPanel;
            }

            UIHelper.SetUIPanelParent(existUIPanel);
            int uiType = existUIPanel.UIPanelType;
            using var componentTypeList = ListComponent<Type>.Create();
            UIHelper.GetUIPanelComponentTypeList(uiType, componentTypeList.List);

            await UIHelper.SetUIPanelEnableAsync(existUIPanel, componentTypeList.List);

            if (existUIPanel.UIPanelAssetEntity.Object.TryGetComponent(out Canvas canvas))
            {
                canvas.enabled = true;
            }

            existUIPanel.IsActive = true;
            return existUIPanel;
        }

        public static async ETTask<UIPanel> OnResumeUIPanel<T>(this UIEventComponent self, UIPanel existUIPanel, T args)
        {
            if (existUIPanel.IsActive)
            {
                return existUIPanel;
            }

            UIHelper.SetUIPanelParent(existUIPanel);
            int uiType = existUIPanel.UIPanelType;

            using var componentTypeList = ListComponent<Type>.Create();
            UIHelper.GetUIPanelComponentTypeList(uiType, componentTypeList.List);

            await UIHelper.SetUIPanelEnableAsync(existUIPanel, componentTypeList.List, args);

            if (existUIPanel.UIPanelAssetEntity.Object.TryGetComponent(out Canvas canvas))
            {
                canvas.enabled = true;
            }

            existUIPanel.IsActive = true;
            return existUIPanel;
        }

        public static async ETTask OnPauseUIPanel(this UIEventComponent self, UIPanel existUIPanel)
        {
            if (!(existUIPanel is { IsActive: true }))
            {
                return;
            }

            using var componentTypeList = ListComponent<Type>.Create();
            UIHelper.GetUIPanelComponentTypeList(existUIPanel.UIPanelType, componentTypeList.List);

            await UIHelper.SetUIPanelDisableAsync(existUIPanel, componentTypeList.List);
            if (existUIPanel.UIPanelAssetEntity.Object.TryGetComponent(out Canvas canvas))
            {
                canvas.enabled = false;
            }

            existUIPanel.UIPanelAssetEntity.Object.transform.SetParent(self.UILayers[(int) UILayer.Hidden]);
            existUIPanel.IsActive = false;
        }

        public static async ETTask OnRemoveUIPanel(this UIEventComponent self, UIPanel existUIPanel)
        {
            await self.OnPauseUIPanel(existUIPanel);
        }

        public static async ETTask<UIItem> OnCreateUIItem(this UIEventComponent self, UIPanel uiPanel, int uiItemType, Transform parentTransform)
        {
            if (!self.UIItemComponentTypes.TryGetValue(uiItemType, out Type uiItemComponentType))
            {
                Log.Error($"UIType:{uiItemType.ToString()} 对应的UIItemComponent未找到！");
                return null;
            }

            UIItemConfig uiItemConfig = UIItemConfigCategory.Instance.Get(uiItemType);
            AssetEntity assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiItemConfig.AssetPath, parentTransform);
            UIItem uiItem = EntityFactory.CreateWithParent<UIItem, int, AssetEntity>(uiPanel, uiItemType, assetEntity);
            uiItem.AddComponent(uiItemComponentType);
            await EventSystem.Instance.EnableAsync(uiItem.GetComponent(uiItemComponentType));
            if (uiItem.UIItemAssetEntity.Object.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup.alpha = 1;
            }

            return uiItem;
        }

        public static async ETTask<UIItem> OnCreateUIItem<T>(this UIEventComponent self, UIPanel uiPanel, int uiItemType, T args,
        Transform parentTransform)
        {
            if (!self.UIItemComponentTypes.TryGetValue(uiItemType, out Type uiItemComponentType))
            {
                Log.Error($"UIType:{uiItemType.ToString()} 对应的UIItemComponent未找到！");
                return null;
            }

            UIItemConfig uiItemConfig = UIItemConfigCategory.Instance.Get(uiItemType);
            AssetEntity assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiItemConfig.AssetPath, parentTransform);
            UIItem uiItem = EntityFactory.CreateWithParent<UIItem, int, AssetEntity>(uiPanel, uiItemType, assetEntity);
            uiItem.AddComponent(uiItemComponentType);
            await EventSystem.Instance.EnableAsync(uiItem.GetComponent(uiItemComponentType), args);
            if (uiItem.UIItemAssetEntity.Object.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup.alpha = 1;
            }

            return uiItem;
        }
    }
}