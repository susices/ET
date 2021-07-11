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
    }
}