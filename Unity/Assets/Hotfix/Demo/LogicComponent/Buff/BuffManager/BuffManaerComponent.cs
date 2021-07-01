using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// Buff管理器组件
    /// </summary>
    public class BuffManaerComponent :Entity
    {
        /// <summary>
        /// 当前持有的buff
        /// key: EntityId
        /// </summary>
        public Dictionary<long, BuffEntity> idBuffEntities;
    }
}