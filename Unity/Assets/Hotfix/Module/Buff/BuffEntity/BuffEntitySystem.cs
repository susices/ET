namespace ET
{
    public class BuffEntityAwakeSystem: AwakeSystem<BuffEntity, Entity, int>
    {
        public override void Awake(BuffEntity self, Entity uiPanelType, int buffConfigId)
        {
            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffConfigId);
            self.SourceEntity = uiPanelType;
            self.BuffContainer = self.Parent as BuffContainerComponent;
            self.BuffConfigId = buffConfigId;
            self.BuffEndTime = TimeHelper.ServerNow() + buffConfig.DurationMillsecond;
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
            self.RunTickAction(BuffConfigCategory.Instance.Get(self.BuffConfigId).BuffTickTimeSpan);
            Log.Debug($"BuffAdded BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
        }
    }

    public class BuffEntityUpdateSystem: UpdateSystem<BuffEntity>
    {
        public override void Update(BuffEntity self)
        {
            if (TimeHelper.ServerNow() >= self.BuffEndTime)
            {
                self.Dispose();
            }
        }
    }

    public class BuffEntityDestroySystem: DestroySystem<BuffEntity>
    {
        public override void Destroy(BuffEntity self)
        {
            if (self.BuffTickTimerId != null)
            {
                TimerComponent.Instance.Remove(self.BuffTickTimerId.Value);
                self.TickBuffActions.Dispose();
                self.TickBuffActions = null;
                self.TickBuffActionsArgs.Dispose();
                self.TickBuffActionsArgs = null;
            }

            if (TimeHelper.ServerNow() >= self.BuffEndTime)
            {
                BuffActionDispatcher.Instance.RunBuffTimeOutAction(self);
                Log.Debug($"BuffTimeOuted BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }
            else
            {
                BuffActionDispatcher.Instance.RunBuffRemoveAction(self);
                Log.Debug($"BuffRemoved BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }

            self.Clear();
        }
    }

    public static class BuffEntitySystem
    {
        /// <summary>
        /// 执行Buff刷新
        /// </summary>
        /// <param name="self"></param>
        public static void RunRefreshAction(this BuffEntity self)
        {
            BuffActionDispatcher.Instance.RunBuffRefreshAction(self);
            Log.Debug($"BuffRefreshed BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
        }

        /// <summary>
        /// 执行Buff 定时Tick
        /// </summary>
        /// <param name="self"></param>
        /// <param name="timeSpan"></param>
        public static void RunTickAction(this BuffEntity self, int timeSpan)
        {
            if (timeSpan <= 0)
            {
                self.BuffTickTimerId = null;
                return;
            }

            if (BuffConfigCategory.Instance.Get(self.BuffConfigId).BuffTickActions == null)
            {
                return;
            }

            self.TickBuffActions = ListComponent<IBuffAction>.Create();
            self.TickBuffActionsArgs = ListComponent<int[]>.Create();
            if (!BuffActionDispatcher.Instance.GetBuffTickActions(self, self.TickBuffActions.List, self.TickBuffActionsArgs.List))
            {
                self.TickBuffActions.Dispose();
                self.TickBuffActions = null;
                self.TickBuffActionsArgs.Dispose();
                self.TickBuffActionsArgs = null;
                return;
            }
            //立即执行一次
            Tick();
            
            //间隔指定时间执行
            self.BuffTickTimerId = TimerComponent.Instance.NewRepeatedTimer(timeSpan, () =>
            {
                Tick();
                Log.Debug($"BuffTicked BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            });

            void Tick()
            {
                for (int i = 0; i < self.TickBuffActions.List.Count; i++)
                {
                    self.TickBuffActions.List[i].Run(self, self.TickBuffActionsArgs.List[i]);
                }
            }
        }

        public static void Clear(this BuffEntity self)
        {
            self.CurrentLayer = 0;
            self.SourceEntity = null;
            self.BuffConfigId = 0;
            self.BuffEndTime = 0;
            self.BuffContainer = null;
            self.BuffTickTimerId = null;
            self.State = BuffState.None;
        }
    }
}