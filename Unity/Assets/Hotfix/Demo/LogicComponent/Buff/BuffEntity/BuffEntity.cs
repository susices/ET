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
        /// 被添加到的BuffManager
        /// </summary>
        public BuffManaerComponent ParentBuffManager;

        /// <summary>
        /// Buff结束时间 单位毫秒
        /// </summary>
        public long BuffEndTime;

        /// <summary>
        /// Buff轮询计时器Id
        /// </summary>
        public long? BuffTickTimerId;
    }
}