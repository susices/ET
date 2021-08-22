namespace ET
{
    public interface IInteractAction
    {
        /// <summary>
        /// 交互函数接口方法
        /// </summary>
        /// <param name="sourceEntity">交互来源实体</param>
        /// <param name="targetEntity">交互目标实体</param>
        void Run(Entity sourceEntity, Entity targetEntity);
    }
}