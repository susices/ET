using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;

namespace ET
{
    public class UIBagScrollCellView : EnhancedScrollerCellView
    {
        public Text m_bagItemId;

        public Text m_bagItemValue;

        public void SetData(BagItem bagItem)
        {
            this.m_bagItemId.text = bagItem.DataId.ToString();
            this.m_bagItemValue.text = bagItem.DataValue.ToString();
        }
    }
}