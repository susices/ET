using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIBagComponentAwakeSystem:AwakeSystem<UIBagComponent>
    {
        public override void Awake(UIBagComponent self)
        {
            ReferenceCollector rc = self.GetParent<UIPanel>().UIPanelAssetEntity.Object.GetComponent<ReferenceCollector>();
            self.ScrollView = rc.Get<GameObject>("ScrollView").GetComponent<EnhancedScroller>();
            self.CloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            self.UIBagScrollCellViewPrefab = rc.Get<GameObject>("UIBagScrollCellView").GetComponent<UIBagScrollCellView>();
            self.CloseBtn.onClick.AddListener(self.OnCloseBagPanel);
            Unit myUnit = self.DomainScene().GetComponent<UnitComponent>().MyUnit;
            self.PlayerBagComponent = myUnit.GetComponent<BagComponent>();
            self.ScrollView.Delegate = new UIBagScrollView(self);
        }
    }
    
    public class UIBagComponentEnableSystem:EnableSystem<UIBagComponent>
    {
        public override async ETTask Enable(UIBagComponent self)
        {
            self.ScrollView.ReloadData();
            await ETTask.CompletedTask;
        }
    }
    
    public class UIBagComponentDestorySystem:DestroySystem<UIBagComponent>
    {
        public override void Destroy(UIBagComponent self)
        {
            self.CloseBtn.onClick.RemoveAllListeners();
            self.ScrollView.Delegate = null;
            self.ScrollView.ClearAll();
        }
    }

    public static class UIBagComponentSystem
    {
        public static void OnCloseBagPanel(this UIBagComponent self)
        {
            self.DomainScene().RemoveUIPanel(UIPanelType.UIBag).Coroutine();
        }
    }
}