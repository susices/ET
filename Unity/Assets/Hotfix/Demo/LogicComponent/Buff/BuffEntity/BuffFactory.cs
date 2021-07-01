namespace ET
{
    /// <summary>
    /// Buff工厂  构造Buff实体
    /// </summary>
    public static class BuffFactory
    {
        public static BuffEntity Create(BuffManaerComponent buffManaerComponent,Entity sourceEntity, int buffConfigId)
        {
            var buffEntity = EntityFactory.CreateWithParent<BuffEntity, Entity, int>(buffManaerComponent, sourceEntity, buffConfigId);
            return buffEntity;
        }
    }
}