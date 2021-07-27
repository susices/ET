namespace ET
{
    /// <summary>
    /// BuffAction接口  
    /// </summary>
    public interface IBuffAction
    {
        /// <summary>
        /// 执行Action
        /// </summary>
        /// <param name="buffEntity">所属的BuffEntity</param>
        /// <param name="args">参数列表</param>
         void Run(Entity buffEntity, int[] args);
    }
}