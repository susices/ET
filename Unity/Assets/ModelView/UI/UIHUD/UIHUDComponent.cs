
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    /// <summary>
    /// HUD组件
    /// </summary>
    [UIPanelComponent(UIPanelType.UIHUD)]
    public class UIHUDComponent : Entity
    {
        public Button BagBtn;

        public Button TransferMap1Btn;
        
        public Button TransferMap2Btn;
    }
}