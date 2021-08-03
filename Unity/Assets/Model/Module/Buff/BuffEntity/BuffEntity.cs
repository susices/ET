using System.Collections.Generic;

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
        /// Buff状态
        /// </summary>
        public BuffState State;

        /// <summary>
        /// 当前Buff层数
        /// </summary>
        public int CurrentLayer;

        /// <summary>
        /// Buff施加来源的实体
        /// </summary>
        public Entity SourceEntity;

        /// <summary>
        /// 被添加到的Buff容器
        /// </summary>
        public BuffContainerComponent BuffContainer;

        /// <summary>
        /// Buff结束时间 单位毫秒
        /// </summary>
        public long BuffEndTime;

        /// <summary>
        /// Buff轮询计时器Id
        /// </summary>
        public long? BuffTickTimerId;

        /// <summary>
        /// 轮询BuffAction列表组件
        /// </summary>
        public ListComponent<IBuffAction> TickBuffActions;

        /// <summary>
        /// 轮询BuffAction 参数列表组件
        /// </summary>
        public ListComponent<int[]> TickBuffActionsArgs;

    }
}