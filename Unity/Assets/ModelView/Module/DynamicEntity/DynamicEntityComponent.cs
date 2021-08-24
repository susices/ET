using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 动态实体组件
    /// 管理运行时动态创建的实体
    /// </summary>
    public class DynamicEntityComponent : Entity
    {
        public Dictionary<int, Entity> DynamicEntities;
    }
}