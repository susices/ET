namespace ET
{
    /// <summary>
    /// Buff工厂  构造Buff实体
    /// </summary>
    public static class BuffFactory
    {
        public static BuffEntity Create(BuffContainerComponent buffContainer,Entity sourceEntity, int buffConfigId)
        {
            var buffEntity = EntityFactory.CreateWithParent<BuffEntity, Entity, int>(buffContainer, sourceEntity, buffConfigId, true);
            BuffActionDispatcher.Instance.RunBuffAddAction(buffEntity);
            BuffActionDispatcher.Instance.RunBuffTickAction(buffEntity);
            if (BuffConfigCategory.Instance.Get(buffConfigId).DurationMillsecond>0)
            {
                buffEntity.AddComponent<BuffCountDownComponent>();
            }
            return buffEntity;
        }
    }
}