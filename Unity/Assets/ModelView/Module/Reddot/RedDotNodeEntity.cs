using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 红点系统 Node实体
    /// </summary>
    public class RedDotNodeEntity : Entity
    {
        public Dictionary<RangeString, RedDotNodeEntity> ChildrenNodes;

        public string FullPath;

        public string Name;

        public int NodeValue;

        public RedDotNodeEntity ParentNode;
        
        
    }
}