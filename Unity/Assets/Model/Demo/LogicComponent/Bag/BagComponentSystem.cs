namespace ET
{

    public class BagComponentAwakeSystem: AwakeSystem<BagComponent>
    {
        public override void Awake(BagComponent self)
        {
            self.AddComponent<DataSetComponent>();
        }
    }
    
    
    public static class BagComponentSystem
    {
        public static async ETTask UseItem(int itemId, int count)
        {
            await ETTask.CompletedTask;
        }

        public static async ETTask SwitchBagTab(int tab)
        {
            await ETTask.CompletedTask;
        }
    }
}