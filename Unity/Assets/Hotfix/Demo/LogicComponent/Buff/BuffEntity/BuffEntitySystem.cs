namespace ET
{
    public class BuffEntityAwakeSystem : AwakeSystem<BuffEntity, Entity,int>
    {
        public override void Awake(BuffEntity self, Entity uiPanelType, int buffConfigId)
        {
            var buffConfig = BuffConfigCategory.Instance.Get(self.BuffConfigId);
            self.SourceEntity = uiPanelType;
            self.ParentBuffManager = self.Parent as BuffContainerComponent;
            self.BuffConfigId = buffConfigId;
            self.BuffEndTime = TimeHelper.ServerNow() + buffConfig.DurationMillsecond;
            self.CurrentLayer++;
            self.State = (BuffState) buffConfig.State;
        }
    }
    
    public class BuffEntityStartSystem : StartSystem<BuffEntity>
    {
        public override void Start(BuffEntity self)
        {
            BuffActionDispatcher.Instance.RunBuffAddAction(self);
            self.RunTickAction(BuffConfigCategory.Instance.Get(self.BuffConfigId).BuffTickTimeSpan);
        }
    }
    
    public class BuffEntityUpdateSystem : UpdateSystem<BuffEntity>
    {
        public override void Update(BuffEntity self)
        {
            if (TimeHelper.ServerNow()>= self.BuffEndTime)
            {
                self.Dispose();
            }
        }
    }
    
    public class BuffEntityDestroySystem : DestroySystem<BuffEntity>
    {
        public override void Destroy(BuffEntity self)
        {
            if (self.BuffTickTimerId!=null)
            {
                TimerComponent.Instance.Remove(self.BuffTickTimerId.Value);
            }
            
            if (TimeHelper.ServerNow()>= self.BuffEndTime)
            {
                BuffActionDispatcher.Instance.RunBuffTimeOutAction(self);
            }
            else
            {
                BuffActionDispatcher.Instance.RunBuffRemoveAction(self);
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
        }
        
        /// <summary>
        /// 执行Buff 定时Tick
        /// </summary>
        /// <param name="self"></param>
        /// <param name="timeSpan"></param>
        public static void RunTickAction(this BuffEntity self, int timeSpan)
        {
            if (timeSpan==0)
            {
                self.BuffTickTimerId = null;
                return;
            }
            
            BuffActionDispatcher.Instance.GetBuffTickActions(self, out var buffActionList, out var argsList);
            self.BuffTickTimerId = TimerComponent.Instance.NewRepeatedTimer(timeSpan, () =>
            {
                for (int i = 0; i < buffActionList.Count; i++)
                {
                    buffActionList[i].Run(self, argsList[i]);
                }
            });
        }

        public static void Clear(this BuffEntity self)
        {
            self.CurrentLayer = 0;
            self.SourceEntity = null;
            self.BuffConfigId = 0;
            self.BuffEndTime = 0;
            self.ParentBuffManager = null;
            self.BuffTickTimerId = null;
            self.State = BuffState.None;
        }
    }
}