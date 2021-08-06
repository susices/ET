namespace ET
{
    
    public static class TransferComponentSystem
    {
        public static async ETTask Transfer(this TransferComponent self, int transferMapIndex)
        {
            //传送协议
            M2C_Transfer m2CTransfer = (M2C_Transfer) await self.DomainScene().GetComponent<SessionComponent>().Session.Call
                    (new C2M_Transfer() { MapIndex = transferMapIndex });
            
            //切换unity场景
            await EventSystem.Instance.Publish(new EventType.AfterServerTransferUnit() { TransferMapIndex = transferMapIndex });
        }
    }
}