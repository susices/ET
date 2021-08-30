namespace ET
{
    public class AssetEntityPoolCountDownComponentAwakeSystem:AwakeSystem<AssetEntityPoolCountDownComponent,long>
    {
        public override void Awake(AssetEntityPoolCountDownComponent self, long disposeTime)
        {
            self.DisposeTime = disposeTime;
            
        }
    }
    
    public class AssetEntityPoolCountDownComponentUpdateSystem:UpdateSystem<AssetEntityPoolCountDownComponent>
    {
        public override void Update(AssetEntityPoolCountDownComponent self)
        {
            if (TimeHelper.ClientNow() > self.DisposeTime)
            {
                self.Parent.Dispose();
            }
        }
    }
    
    public class AssetEntityPoolCountDownComponentDestroySystem:DestroySystem<AssetEntityPoolCountDownComponent>
    {
        public override void Destroy(AssetEntityPoolCountDownComponent self)
        {
            self.DisposeTime = 0;
        }
    }
}