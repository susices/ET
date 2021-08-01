using System.Collections.Generic;
using UnityEngine;

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
        public GameObject ScrollView;
        /// <summary>
        /// 关闭页面按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 玩家背包组件
        /// </summary>
        public BagComponent PlayerBagComponent;
    }
}