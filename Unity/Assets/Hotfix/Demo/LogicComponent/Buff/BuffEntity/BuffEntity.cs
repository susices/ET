namespace ET
{
    /// <summary>
    /// Buff实体
    /// </summary>
    public class BuffEntity : Entity
    {
        /// <summary>
        /// buff配置表Id
        /// </summary>
        public int BuffConfigId;

        /// <summary>
        /// 当前Buff层数
        /// </summary>
        public int CurrentLayer;

        /// <summary>
        /// Buff施加来源的实体
        /// </summary>
        public Entity SourceEntity;

        /// <summary>
        /// 剩余持续时间秒数
        /// </summary>
        public float LeftDurationSeconds;
        
    }
}