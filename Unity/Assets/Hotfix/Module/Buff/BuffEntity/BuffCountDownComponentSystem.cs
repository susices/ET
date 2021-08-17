namespace ET
{
    public class BuffCountDownComponentAwakeSystem: AwakeSystem<BuffCountDownComponent>
    {
        public override void Awake(BuffCountDownComponent self)
        {
            self.ParentBuffEntity = self.GetParent<BuffEntity>();
            self.BuffConfigId = self.ParentBuffEntity.BuffConfigId;
            self.IsCountDownEnd = false;
            self.CountDown().Coroutine();
        }
    }
    
    public class BuffCountDownComponentDestroySystem:DestroySystem<BuffCountDownComponent>
    {
        public override void Destroy(BuffCountDownComponent self)
        {
            
        }
    }

    public static class BuffCountDownComponentSystem
    {
        public static async ETTask CountDown(this BuffCountDownComponent self)
        {
            if (await TimerComponent.Instance.WaitAsync(BuffConfigCategory.Instance.Get(self.BuffConfigId).DurationMillsecond,self.BuffCountDownCancellationToken))
            {
                BuffActionDispatcher.Instance.RunBuffTimeOutAction(self.ParentBuffEntity);
                self.IsCountDownEnd = true;
                self.ParentBuffEntity.Dispose();
            }
        }
    }
}