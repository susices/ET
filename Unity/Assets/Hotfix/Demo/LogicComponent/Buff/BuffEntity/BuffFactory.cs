namespace ET
{
    /// <summary>
    /// Buff工厂  构造Buff实体
    /// </summary>
    public static class BuffFactory
    {
        public static BuffEntity Create(Entity domain, Entity sourceEntity, BuffManaerComponent buffManaerComponent, int buffConfigId)
        {
            var buffEntity = EntityFactory.Create<BuffEntity, Entity, BuffManaerComponent, int>(domain, sourceEntity, buffManaerComponent, buffConfigId);
            return buffEntity;
        }
    }
}