using System;
using UnityEngine;

namespace ET
{
    public class UIPanelAwakeSystem: AwakeSystem<UIPanel, int, AssetEntity, bool>
    {
        public override void Awake(UIPanel self, int uiPanelType, AssetEntity UIAssetEntity, bool isSubPanel)
        {
            self.UIPanelAssetEntity = UIAssetEntity;
            self.UIPanelAssetEntity.Object.AddComponent<ComponentView>().Component = this;
            self.UIPanelAssetEntity.Object.layer = LayerMask.NameToLayer(LayerNames.UI);
            self.UIPanelType = uiPanelType;
            self.IsSubPanel = isSubPanel;
        }
    }

    public static class UIPanelSystem
    {
        /// <summary>
        /// 设至显示层级为最上
        /// </summary>
        public static void SetAsFirstSibling(this UIPanel self)
        {
            self.UIPanelAssetEntity.Object.transform.SetAsFirstSibling();
        }

        /// <summary>
        /// 显示SubPanel  无参数
        /// </summary>
        public static async ETTask<UIPanel> ShowSubPanel(this UIPanel self, int uiPanelType)
        {
            if (self.GetComponent<UIPanelComponent>() == null)
            {
                self.AddComponent<UIPanelComponent>();
            }
            return await self.GetComponent<UIPanelComponent>().ShowUIPanel(uiPanelType,true);
        }
        
        /// <summary>
        /// 显示SubPanel  泛型参数
        /// </summary>
        public static async ETTask<UIPanel> ShowSubPanel<T>(this UIPanel self, int uiPanelType, T args)
        {
            if (self.GetComponent<UIPanelComponent>() == null)
            {
                self.AddComponent<UIPanelComponent>();
            }
            return await self.GetComponent<UIPanelComponent>().ShowUIPanel(uiPanelType,true,args);
        }

        /// <summary>
        /// 隐藏SubPanel
        /// </summary>
        public static async ETTask HideSubPanel(this UIPanel self, int uiPanelType)
        {
            if (self.GetComponent<UIPanelComponent>() == null)
            {
                Log.Error("当前UIPanel 没有UIPanelComponent组件！");
                return;
            }

            await self.GetComponent<UIPanelComponent>().HideUIPanel(uiPanelType);
        }

        /// <summary>
        /// 移除SubPanel
        /// </summary>
        public static async ETTask RemoveSubPanel(this UIPanel self, int uiPanelType)
        {
            if (self.GetComponent<UIPanelComponent>() == null)
            {
                Log.Error("当前UIPanel 没有UIPanelComponent组件！");
                return;
            }

            await self.GetComponent<UIPanelComponent>().RemoveUIPanel(uiPanelType);
        }

        /// <summary>
        /// 获取SubPanel
        /// </summary>
        public static UIPanel GetSubPanel(this UIPanel self, int uiPanelType)
        {
            if (self.GetComponent<UIPanelComponent>() == null)
            {
                Log.Error("当前UIPanel 没有UIPanelComponent组件！");
                return null;
            }

            return self.GetComponent<UIPanelComponent>().Get(uiPanelType);
        }

        /// <summary>
        /// 创建指定类型的的UIItem 无参数
        /// </summary>
        public static async ETTask<UIItem> CreateUIItem(this UIPanel self, int uiItemType, Transform parentTransform)
        {
            if (!UIEventComponent.Instance.UIItemTypes.TryGetValue(uiItemType, out Type uiItemComponentType))
            {
                Log.Error($"UIType:{uiItemType.ToString()} 对应的UIItemComponent未找到！");
                return null;
            }
            UIItemConfig uiItemConfig = UIItemConfigCategory.Instance.Get(uiItemType);
            AssetEntity AssetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiItemConfig.AssetPath, parentTransform);
            UIItem uiItem = EntityFactory.CreateWithParent<UIItem, int, AssetEntity>(self, uiItemType, AssetEntity);
            uiItem.AddComponent(uiItemComponentType);
            await EventSystem.Instance.EnableAsync(uiItem.GetComponent(uiItemComponentType));
            if (uiItem.UIItemAssetEntity.Object.TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.alpha = 1;
            }
            return uiItem;
        }
        
        /// <summary>
        /// 创建指定类型的的UIItem 泛型参数
        /// </summary>
        public static async ETTask<UIItem> CreateUIItem<T>(this UIPanel self, int uiItemType, T args, Transform parentTransform)
        {
            if (!UIEventComponent.Instance.UIItemTypes.TryGetValue(uiItemType, out Type uiItemComponentType))
            {
                Log.Error($"UIType:{uiItemType.ToString()} 对应的UIItemComponent未找到！");
                return null;
            }
            UIItemConfig uiItemConfig = UIItemConfigCategory.Instance.Get(uiItemType);
            AssetEntity AssetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiItemConfig.AssetPath, parentTransform);
            UIItem uiItem = EntityFactory.CreateWithParent<UIItem, int, AssetEntity>(self, uiItemType, AssetEntity);
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