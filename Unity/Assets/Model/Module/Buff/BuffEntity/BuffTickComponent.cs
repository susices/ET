namespace ET
{
    /// <summary>
    /// buff实体轮询组件
    /// </summary>
    public class BuffTickComponent:Entity
    {
        /// <summary>
        /// buff配置表Id
        /// </summary>
        public int BuffConfigId;
        
        /// <summary>
        /// buff轮询计时器Id
        /// </summary>
        public long BuffTickTimerId;
        
        /// <summary>
        /// 轮询BuffAction列表组件
        /// </summary>
        public ListComponent<IBuffAction> TickBuffActions;

        /// <summary>
        /// 轮询BuffAction 参数列表组件
        /// </summary>
        public ListComponent<int[]> TickBuffActionsArgs;

        /// <summary>
        /// 所属Buff实体
        /// </summary>
        public BuffEntity ParentBuffEntity;
    }
}