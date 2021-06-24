namespace ET
{

    [ObjectSystem]
    public class BagComponentAwakeSystem: AwakeSystem<BagComponent>
    {
        public override void Awake(BagComponent self)
        {
            self.AddComponent<DataSetComponent,DataType>(DataType.BagItem);
        }
    }
    
    
    public static class BagComponentSystem
    {
        public static async ETTask GetAllBagItem(this BagComponent self)
        {
            var sessioncomponent = self.DomainScene().GetComponent<SessionComponent>();
            var session = sessioncomponent.Session;
            var m2cAllBagInfo = (M2C_AllBagInfo)await session.Call(new C2M_AllBagInfo());
            self.GetComponent<DataSetComponent>().UpdateData(DataUpdateMode.Overwrite, m2cAllBagInfo.BagItems);
            await ETTask.CompletedTask;
        }
        
        public static async ETTask UseItem(this BagComponent self, int itemId, int count)
        {
            
            await ETTask.CompletedTask;
        }

        public static async ETTask SwitchBagTab(this BagComponent self, int tab)
        {
            
            await ETTask.CompletedTask;
        }
    }
}