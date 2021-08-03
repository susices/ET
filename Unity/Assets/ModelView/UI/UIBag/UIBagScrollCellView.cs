using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;

namespace ET
{
    public class UIBagScrollCellView : EnhancedScrollerCellView
    {
        public BagItem m_bagItem;
        public Text m_bagItemId;

        public Text m_bagItemValue;

        public Button m_useBtn;

        public void SetData(BagItem bagItem)
        {
            m_bagItem = bagItem;
            this.m_bagItemId.text = $"Id: {m_bagItem.DataId.ToString()}";
            this.m_bagItemValue.text = $"Count: {m_bagItem.DataValue.ToString()}";
        }
    }
}