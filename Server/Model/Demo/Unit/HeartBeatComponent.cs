namespace ET
{
    /// <summary>
    /// 心跳组件
    /// 一定时间无心跳 断开Session 清理数据
    /// </summary>
    public class HeartBeatComponent: Entity
    {
        /// <summary>
        /// 上一次收到客户端Ping消息的时间
        /// </summary>
        public long LastPingTime;

        /// <summary>
        /// 断线间隔时间
        /// 超过此时间未收到ping消息 会视为断线
        /// </summary>
        public long DisconnectDeltaTime;

        /// <summary>
        /// 轮询检查的间隔时间
        /// </summary>
        public long CheckDeltaTime;

        /// <summary>
        /// 上一次检查的时间
        /// </summary>
        public long LastCheckTime;
    }
}