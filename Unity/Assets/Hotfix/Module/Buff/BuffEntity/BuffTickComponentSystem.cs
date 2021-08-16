namespace ET
{
    public class BuffTickComponentAwakeSystem:AwakeSystem<BuffTickComponent,int>
    {
        public override void Awake(BuffTickComponent self, int buffConfigId)
        {
            self.BuffConfigId = buffConfigId;
            self.BuffTickTimeSpan = BuffConfigCategory.Instance.Get(buffConfigId).BuffTickTimeSpan;
            self.ParentBuffEntity = self.GetParent<BuffEntity>();
            self.TickBuffActions = ListComponent<IBuffAction>.Create();
            self.TickBuffActionsArgs = ListComponent<int[]>.Create();
            if (!BuffActionDispatcher.Instance.GetBuffTickActions(self.GetParent<BuffEntity>(), self.TickBuffActions.List, self.TickBuffActionsArgs.List))
            {
                self.Dispose();
            }
            self.StartTick();
        }
    }

    public class BuffTickComponentDestroySystem:DestroySystem<BuffTickComponent>
    {
        public override void Destroy(BuffTickComponent self)
        {
            TimerComponent.Instance.Remove(self.BuffTickTimerId);
            self.BuffTickTimerId = 0;
            self.BuffTickTimeSpan = 0;
            self.TickBuffActions.Dispose();
            self.TickBuffActions = null;
            self.TickBuffActionsArgs.Dispose();
            self.TickBuffActionsArgs = null;
        }
    }

    public static class BuffTickComponentSystem
    {
        public static void StartTick(this BuffTickComponent self)
        {
            self.Tick();
            self.BuffTickTimerId = TimerComponent.Instance.NewRepeatedTimer(self.BuffTickTimeSpan, () =>
            {
                self.Tick();
                Log.Debug($"BuffTicked BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            });
        }
        
        public static void Tick(this BuffTickComponent self)
        {
            for (int i = 0; i < self.TickBuffActions.List.Count; i++)
            {
                self.TickBuffActions.List[i].Run(self.ParentBuffEntity, self.TickBuffActionsArgs.List[i]);
            }
        }
    }
}