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
        /// </summary>
        public Dictionary<long, BuffEntity> idBuffEntities;

        /// <summary>
        ///  buff管理器影响的实体 
        /// </summary>
        public Entity parentEntity;
    }
}