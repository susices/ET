namespace ET
{
    public class BuffEntityAwakeSystem: AwakeSystem<BuffEntity, Entity, int>
    {
        public override void Awake(BuffEntity self, Entity sourceEntity, int buffConfigId)
        {
            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffConfigId);
            self.SourceEntity = sourceEntity;
            self.BuffContainer = self.Parent as BuffContainerComponent;
            self.BuffConfigId = buffConfigId;
            self.CurrentLayer++;
            self.State = (BuffState) buffConfig.State;
            Log.Debug($"BuffAwaked BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
        }
    }

    public class BuffEntityStartSystem: StartSystem<BuffEntity>
    {
        public override void Start(BuffEntity self)
        {
            BuffActionDispatcher.Instance.RunBuffAddAction(self);
            Log.Debug($"BuffAdded BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            self.TryRunTick();
            self.TryRunCountDown();
        }
    }

    public class BuffEntityDestroySystem: DestroySystem<BuffEntity>
    {
        public override void Destroy(BuffEntity self)
        {
            if (TimeHelper.ServerNow() >= self.BuffEndTime)
            {
                BuffActionDispatcher.Instance.RunBuffTimeOutAction(self);
                Log.Debug($"BuffTimeOuted BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }

            if (self.GetComponent<BuffCountDownComponent>()==null)
            {
                BuffActionDispatcher.Instance.RunBuffRemoveAction(self);
                Log.Debug($"BuffRemoved BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }
            
            self.SetContainerBuffStateOnRemove();

            self.Clear();
        }
    }

    public static class BuffEntitySystem
    {
        /// <summary>
        /// 尝试运行Buff轮询
        /// </summary>
        /// <param name="self"></param>
        public static void TryRunTick(this BuffEntity self)
        {
            if (BuffConfigCategory.Instance.Get(self.BuffConfigId).BuffTickActions == null)
            {
                return;
            }

            if (BuffConfigCategory.Instance.Get(self.BuffConfigId).BuffTickTimeSpan<=0)
            {
                Log.Error($"BuffConfig Tick间隔时间配置错误！ BuffConfigId: {self.BuffConfigId.ToString()}");
                return;
            }
            self.AddComponent<BuffTickComponent>();
        }

        public static void TryRunCountDown(this BuffEntity self)
        {
            if (BuffConfigCategory.Instance.Get(self.BuffConfigId).DurationMillsecond<=0)
            {
                return;
            }
            
            
        }
        
        /// <summary>
        /// 执行Buff刷新
        /// </summary>
        /// <param name="self"></param>
        public static void RunRefreshAction(this BuffEntity self)
        {
            BuffActionDispatcher.Instance.RunBuffRefreshAction(self);
            Log.Debug($"BuffRefreshed BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
        }

        public static void Clear(this BuffEntity self)
        {
            self.CurrentLayer = 0;
            self.SourceEntity = null;
            self.BuffConfigId = 0;
            self.BuffEndTime = 0;
            self.BuffContainer = null;
            self.State = BuffState.None;
        }
        
        public static void SetContainerBuffStateOnRemove(this BuffEntity self)
        {
            foreach (var child in self.GetParent<BuffContainerComponent>().Children.Values)
            {
                if (child is BuffEntity buffEntity && buffEntity.Id!= self.Id)
                {
                    if (buffEntity.State == self.State)
                    {
                        return;
                    }
                }
            }
            var buffContainer = self.GetParent<BuffContainerComponent>();
            buffContainer.BuffState = buffContainer.BuffState & (~self.State);
        }
    }
}