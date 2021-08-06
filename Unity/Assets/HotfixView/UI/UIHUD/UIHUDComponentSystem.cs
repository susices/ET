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
            self.TransferBtn = rc.Get<GameObject>("TransferBtn").GetComponent<Button>();
            self.BagBtn.onClick.AddListener(self.OnOpenBagPanel);
            self.TransferBtn.onClick.AddListener(self.OnTransfer);
        }
    }

    public static class UIHUDComponentSystem
    {
        public static void OnOpenBagPanel(this UIHUDComponent self)
        {
            self.DomainScene().ShowUIPanel(UIPanelType.UIBag).Coroutine();
        }

        public static void OnTransfer(this UIHUDComponent self)
        {
            self.DomainScene().GetComponent<TransferComponent>().Transfer(2).Coroutine();
        }
    }
}