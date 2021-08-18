using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{

    [ObjectSystem]
    public class BagComponentAwakeSystem: AwakeSystem<BagComponent>
    {
        public override void Awake(BagComponent self)
        {
            self.AddComponent<DataSetComponent,DataType>(DataType.BagItem);
            self.BagItemDataSet = self.GetComponent<DataSetComponent>().DataSet;
        }
    }

    public static class BagComponentSystem
    {
        public static async ETTask GetAllBagItem(this BagComponent self)
        {
            var sessioncomponent = self.DomainScene().GetComponent<SessionComponent>();
            var session = sessioncomponent.Session;
            var m2cAllBagInfo = (M2C_AllBagInfo)await session.Call(new C2M_AllBagInfo());
            await self.GetComponent<DataSetComponent>().UpdateData(DataUpdateMode.Overwrite, m2cAllBagInfo.BagItems);
        }
        
        public static async ETTask UseItem(this BagComponent self, int itemId, int count)
        {
            var sessioncomponent = self.DomainScene().GetComponent<SessionComponent>();
            var session = sessioncomponent.Session;
            var m2cUseBatItem = (M2C_UseBagItem) await session.Call(new C2M_UseBagItem()
            {
                BagItemId = itemId,
                BagItemCount = count
            });
            if (m2cUseBatItem.Error != ErrorCode.ERR_Success)
            {
                Log.Error("使用物品失败！");
                return;
            }
            await self.GetComponent<DataSetComponent>().UpdateData(DataUpdateMode.Difference, m2cUseBatItem.BagItems);
            if (itemId==1)
            {
                self.GetParent<Unit>().GetComponent<BuffContainerComponent>().TryAddBuff(1, self);
            }
            else
            {
                self.GetParent<Unit>().GetComponent<BuffContainerComponent>().TryRemoveBuff(1);
            }
        }

        public static async ETTask SwitchBagTab(this BagComponent self, int tab)
        {
            await ETTask.CompletedTask;
        }
    }
}