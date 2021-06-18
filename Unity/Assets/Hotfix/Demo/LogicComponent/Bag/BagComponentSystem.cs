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