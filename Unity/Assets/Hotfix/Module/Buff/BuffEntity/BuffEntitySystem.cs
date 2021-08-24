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

    public class BuffEntityDestroySystem: DestroySystem<BuffEntity>
    {
        public override void Destroy(BuffEntity self)
        {
            Log.Info($"BuffEntity Destroyed BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            self.SetContainerBuffStateOnRemove();
            self.Clear();
        }
    }

    public static class BuffEntitySystem
    {

        public static void Clear(this BuffEntity self)
        {
            self.CurrentLayer = 0;
            self.SourceEntity = null;
            self.BuffConfigId = 0;
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