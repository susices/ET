using UnityEngine;
using UnityEngine.UI;

namespace ET.UIHUD
{
    public class UIHUDComponentAwakeSystem: AwakeSystem<UIHUDComponent>
    {
        public override void Awake(UIHUDComponent self)
        {
            ReferenceCollector rc = self.GetParent<UIPanel>().UIPanelAssetEntity.Object.GetComponent<ReferenceCollector>();
            self.BagBtn = rc.Get<GameObject>("BagBtn").GetComponent<Button>();
            self.TransferMap1Btn = rc.Get<GameObject>("TransferMap1Btn").GetComponent<Button>();
            self.TransferMap2Btn = rc.Get<GameObject>("TransferMap2Btn").GetComponent<Button>();
            self.BagBtn.onClick.AddListener(self.OnOpenBagPanel);
            self.TransferMap1Btn.onClick.AddListener(self.OnTransferMap1);
            self.TransferMap2Btn.onClick.AddListener(self.OnTransferMap2);
        }
    }

    public static class UIHUDComponentSystem
    {
        public static void OnOpenBagPanel(this UIHUDComponent self)
        {
            self.DomainScene().ShowUIPanel(UIPanelType.UIBag).Coroutine();
        }
        
        public static void OnTransferMap1(this UIHUDComponent self)
        {
            
            self.DomainScene().GetComponent<TransferComponent>().Transfer(1, new Vector3(-10,0,-10)).Coroutine();
        }
        

        public static void OnTransferMap2(this UIHUDComponent self)
        {
            self.DomainScene().GetComponent<TransferComponent>().Transfer(2, new Vector3(10,0,10)).Coroutine();
        }
    }
}