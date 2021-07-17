using System;
using System.Collections.Generic;

namespace ET
{
    
    public static class UIHelper
    {
        /// <summary>
        /// 设置UIPanel所属GameObject的父节点
        /// </summary>
        /// <param name="uiPanel"></param>
        public static void SetUIPanelParent(UIPanel uiPanel)
        {
            if (uiPanel.IsSubPanel)
            {
                var parentUIPanel = uiPanel.GetParent<UIPanelComponent>().GetParent<UIPanel>();
                uiPanel.UIPanelAssetEntity.Object.transform.SetParent(parentUIPanel.UIPanelAssetEntity.Object.transform);
            }
            else
            {
                var uiLayer = UIPanelConfigCategory.Instance.Get(uiPanel.UIPanelType).UILayer;
                uiPanel.UIPanelAssetEntity.Object.transform.SetParent(UIEventComponent.Instance.UILayers[uiLayer]);
            }
        }

        /// <summary>
        /// 根据UIPanelType获取UIPanelComponentType列表
        /// </summary>
        public static void GetUIPanelComponentTypeList(int uiPanelType,List<Type> typeList)
        {
            UIPanelConfig uiConfig = UIPanelConfigCategory.Instance.Get(uiPanelType);
            
            foreach (int componentIndex in uiConfig.UIPanelComponentIndexs)
            {
                if (!UIEventComponent.Instance.UIPanelComponentTypes.TryGetValue(componentIndex, out var UIComponentType))
                {
                    Log.Error($"UIType:{uiPanelType.ToString()} 对应的UIPanelComponent未找到！");
                    return;
                }
                typeList.Add(UIComponentType);
            }
        }

        /// <summary>
        /// 运行UIPanel的组件enable方法  无参数
        /// </summary>
        public static async ETTask SetUIPanelEnableAsync(UIPanel uiPanel, List<Type> uiPanelComponentTypes)
        {
            using var tcsList = ListComponent<ETTask>.Create();
            foreach (var uiComponentTye in uiPanelComponentTypes)
            {
                tcsList.List.Add(EventSystem.Instance.EnableAsync(uiPanel.GetComponent(uiComponentTye)));
            } 
            await ETTaskHelper.WaitAll(tcsList.List);
        }
        
        /// <summary>
        /// 运行UIPanel的组件enable方法  泛型参数
        /// </summary>
        public static async ETTask SetUIPanelEnableAsync<T>(UIPanel uiPanel, List<Type> uiPanelComponentTypes,T args)
        {
            using var tcsList = ListComponent<ETTask>.Create();
            foreach (var uiComponentTye in uiPanelComponentTypes)
            {
                tcsList.List.Add(EventSystem.Instance.EnableAsync(uiPanel.GetComponent(uiComponentTye),args));
            } 
            await ETTaskHelper.WaitAll(tcsList.List);
        }
        
        /// <summary>
        /// 运行UIPanel的组件disable方法
        /// </summary>
        public static async ETTask SetUIPanelDisableAsync(UIPanel uiPanel, List<Type> uiPanelComponentTypes)
        {
            using var tcsList = ListComponent<ETTask>.Create();
            foreach (var uiComponentTye in uiPanelComponentTypes)
            {
                tcsList.List.Add(EventSystem.Instance.DisableAsync(uiPanel.GetComponent(uiComponentTye)));
            } 
            await ETTaskHelper.WaitAll(tcsList.List);
        }
        
        
        
    }
}