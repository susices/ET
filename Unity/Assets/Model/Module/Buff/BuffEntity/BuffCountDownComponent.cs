namespace ET
{
    /// <summary>
    /// buff实体倒计时组件
    /// </summary>
    public class BuffCountDownComponent:Entity
    {
        /// <summary>
        /// buff配置表Id
        /// </summary>
        public int BuffConfigId;

        /// <summary>
        /// 所属Buff实体
        /// </summary>
        public BuffEntity ParentBuffEntity;

        /// <summary>
        /// 记录是否已倒计时结束
        /// </summary>
        public bool IsCountDownEnd;

        /// <summary>
        /// buff倒计时取消Token
        /// </summary>
        public ETCancellationToken BuffCountDownCancellationToken = new ETCancellationToken();
    }
}