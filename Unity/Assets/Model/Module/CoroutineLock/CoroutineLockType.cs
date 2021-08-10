namespace ET
{
    public enum CoroutineLockType
    {
        None = 0,
        /// <summary>
        /// location进程上使用
        /// </summary>
        Location, 
        /// <summary>
        /// ActorLocationSender中队列消息 
        /// </summary>
        ActorLocationSender,
        /// <summary>
        /// Mailbox中队列
        /// </summary>
        Mailbox, 
        /// <summary>
        /// 存储数据库
        /// </summary>
        DB,
        /// <summary>
        /// 数据缓存
        /// </summary>
        DBCache,
        /// <summary>
        /// 资源
        /// </summary>
        Resources,
        /// <summary>
        /// 背包
        /// </summary>
        Bag,
        //必须放最后
        Max,
    }
}