using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    /// <summary>
    /// 背包UI组件
    /// </summary>
    [UIPanelComponent(UIPanelType.UIBag)]
    public class UIBagComponent :Entity
    {
        /// <summary>
        /// 背包物品列表
        /// </summary>
        public EnhancedScroller ScrollView;
        /// <summary>
        /// 关闭页面按钮
        /// </summary>
        public Button CloseBtn;
        /// <summary>
        /// 玩家背包组件
        /// </summary>
        public BagComponent PlayerBagComponent;
        /// <summary>
        /// 背包物品UIPrefab
        /// </summary>
        public UIBagScrollCellView UIBagScrollCellViewPrefab;
    }
}