namespace ET
{
    /// <summary>
    /// Buff工厂  构造Buff实体
    /// </summary>
    public static class BuffFactory
    {
        public static BuffEntity Create(BuffContainerComponent buffManagerComponent,Entity sourceEntity, int buffConfigId)
        {
            var buffEntity = EntityFactory.CreateWithParent<BuffEntity, Entity, int>(buffManagerComponent, sourceEntity, buffConfigId, true);
            return buffEntity;
        }
    }
}