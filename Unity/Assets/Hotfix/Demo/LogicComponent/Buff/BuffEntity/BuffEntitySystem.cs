namespace ET
{
    public class BuffEntityAwakeSystem : AwakeSystem<BuffEntity, Entity,int>
    {
        public override void Awake(BuffEntity self, Entity sourceEntity, int buffConfigId)
        {
            self.SourceEntity = sourceEntity;
            self.ParentBuffManager = self.Parent as BuffManaerComponent;
            self.BuffConfigId = buffConfigId;
            self.BuffEndTime = TimeHelper.ServerNow() + BuffConfigCategory.Instance.Get(self.BuffConfigId).DurationMillsecond;
        }
    }
    
    public class BuffEntityStartSystem : StartSystem<BuffEntity>
    {
        public override void Start(BuffEntity self)
        {
            BuffActionComponent.Instance.RunBuffAddAction(self);
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
                BuffActionComponent.Instance.RunBuffTimeOutAction(self);
            }
            else
            {
                BuffActionComponent.Instance.RunBuffRemoveAction(self);
            }
        }
    }

    public static class BuffEntitySystem
    {
        public static void RunTickAction(this BuffEntity self, int timeSpan)
        {
            if (timeSpan==0)
            {
                self.BuffTickTimerId = null;
                return;
            }
            
            BuffActionComponent.Instance.GetBuffTickActions(self, out var buffActionList, out var argsList);
            self.BuffTickTimerId = TimerComponent.Instance.NewRepeatedTimer(timeSpan, () =>
            {
                for (int i = 0; i < buffActionList.Count; i++)
                {
                    buffActionList[i].Run(self, argsList[i]);
                }
            });
        }
    }
}