using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// Buff容器组件
    /// </summary>
    public class BuffContainerComponent :Entity
    {
        /// <summary>
        /// 当前持有的buff
        /// key: EntityId
        /// </summary>
        public Dictionary<long, BuffEntity> idBuffEntities;
    }
}