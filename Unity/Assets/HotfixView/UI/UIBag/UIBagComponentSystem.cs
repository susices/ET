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
            var uiBagScrollView = new UIBagScrollView(self);
            self.ScrollView.Delegate = uiBagScrollView;
            self.ScrollView.cellViewInstantiated += uiBagScrollView.AfterCellViewCreated;
        }
    }
    
    public class UIBagComponentEnableSystem:EnableSystem<UIBagComponent>
    {
        public override async ETTask Enable(UIBagComponent self)
        {
            DataUpdateComponent.Instance.AddListener(DataType.BagItem, self);
            self.ScrollView.ReloadData();
            await ETTask.CompletedTask;
        }
    }
    
    public class UIBagComponentDisableSystem:DisableSystem<UIBagComponent>
    {
        public override async ETTask Disable(UIBagComponent self)
        {
            DataUpdateComponent.Instance.RemoveListener(DataType.BagItem, self);
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
        
        public static void OnUseBagItem(this UIBagComponent self, int bagItemId)
        {
            self.PlayerBagComponent.UseItem(bagItemId,1).Coroutine();
            if (bagItemId==1)
            {
                SceneEntityComponent.Instance.LoadSceneEntities(1001).Coroutine();
            }
            else
            {
                SceneEntityComponent.Instance.UnLoadSceneEntities(1001).Coroutine();
            }
            
        }

        public static void OnDataUpdate(this UIBagComponent self)
        {
            self.ScrollView.ReloadData();
        }
    }
}