using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;

namespace ET
{
    public class UIBagScrollView:IEnhancedScrollerDelegate
    {
        public UIBagComponent m_UIBagComponent;
        public UIBagScrollView(UIBagComponent uiBagComponent)
        {
            this.m_UIBagComponent = uiBagComponent;
        }
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return this.m_UIBagComponent.PlayerBagComponent.BagItemDataSet.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 100f;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            UIBagScrollCellView cellView = scroller.GetCellView(this.m_UIBagComponent.UIBagScrollCellViewPrefab) as UIBagScrollCellView;
            cellView.name = $"BagCellView {dataIndex.ToString()}";
            cellView.SetData(this.m_UIBagComponent.PlayerBagComponent.BagItemDataSet.Values[dataIndex] as BagItem);
            return cellView;
        }
    }
}