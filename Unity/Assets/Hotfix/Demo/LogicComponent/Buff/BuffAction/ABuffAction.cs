namespace ET
{
    /// <summary>
    /// BuffAction抽象类  
    /// </summary>
    public abstract class ABuffAction
    {
        /// <summary>
        /// 执行Action
        /// </summary>
        /// <param name="buffEntity">所属的BuffEntity</param>
        /// <param name="args">参数列表</param>
        public abstract void Run(Entity buffEntity, int[] args);
    }
}