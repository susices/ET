namespace ET
{
    public class BuffEntityAwakeSystem : AwakeSystem<BuffEntity, Entity, BuffManaerComponent,int>
    {
        public override void Awake(BuffEntity self, Entity sourceEntity, BuffManaerComponent buffManaerComponent, int buffConfigId)
        {
            self.SourceEntity = sourceEntity;
            self.BuffManager = buffManaerComponent;
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
                self.BuffManager.RemoveBuff(self.Id);
            }
        }
    }
    
    public class BuffEntityDestroySystem : DestroySystem<BuffEntity>
    {
        public override void Destroy(BuffEntity self)
        {
            if (TimeHelper.ServerNow()>= self.BuffEndTime)
            {
                BuffActionComponent.Instance.RunBuffTimeOutAction(self);
            }
            else
            {
                BuffActionComponent.Instance.RunBuffRemoveAction(self);
            }
            TimerComponent.Instance.Remove(self.BuffTickTimerId);
        }
    }

    public static class BuffEntitySystem
    {
        public static void RunTickAction(this BuffEntity self, int timeSpan)
        {
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