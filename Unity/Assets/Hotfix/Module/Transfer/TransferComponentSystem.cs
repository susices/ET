using UnityEngine;

namespace ET
{
    
    public static class TransferComponentSystem
    {
        public static async ETTask Transfer(this TransferComponent self, int transferMapIndex, Vector3 transferPos)
        {
            if (self.CurrentMapIndex == transferMapIndex)
            {
                Log.Debug("不能传送到当前地图！");
                return;
            }
            //传送前先停止移动 不然序列化会出错
            self.DomainScene().GetComponent<SessionComponent>().Session.Send(new C2M_Stop());
            
            var unitId = self.DomainScene().GetComponent<UnitComponent>().MyUnit.Id;
            //传送协议
            M2C_Transfer m2CTransfer = (M2C_Transfer) await self.DomainScene().GetComponent<SessionComponent>().Session.Call
                    (new C2M_Transfer() { MapIndex = transferMapIndex, X = transferPos.x,Y = transferPos.y,Z = transferPos.z});
            self.CurrentMapIndex = transferMapIndex;
            //切换unity场景
            await EventSystem.Instance.Publish(new EventType.AfterServerTransferUnit() 
                    { DomainScene = self.DomainScene(),
                        TransferMapIndex = transferMapIndex, 
                        UnitId = unitId}); 
            
        }
    }
}