using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace ET
{
    public class UIBagComponentAwakeSystem:AwakeSystem<UIBagComponent>, IEnhancedScrollerDelegate
    {
        public override void Awake(UIBagComponent self)
        {
            ReferenceCollector rc = self.GetParent<UIPanel>().UIPanelAssetEntity.Object.GetComponent<ReferenceCollector>();
            self.ScrollView = rc.Get<GameObject>("ScrollView");
            self.CloseBtn = rc.Get<GameObject>("CloseBtn");
            Unit myUnit = self.DomainScene().GetComponent<UnitComponent>().MyUnit;
            self.PlayerBagComponent = myUnit.GetComponent<BagComponent>();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            throw new System.NotImplementedException();
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            throw new System.NotImplementedException();
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            throw new System.NotImplementedException();
        }
    }

    public static class UIBagComponentSystem
    {
        
    }
}