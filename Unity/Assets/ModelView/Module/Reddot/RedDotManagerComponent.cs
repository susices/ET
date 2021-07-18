using System.Collections.Generic;
using System.Text;

namespace ET
{
    /// <summary>
    /// 红点系统管理器组件
    /// </summary>
    public class RedDotManagerComponent : Entity
    {
        public static RedDotManagerComponent Instance;

        /// <summary>
        /// 所有节点集合
        /// </summary>
        public Dictionary<string, RedDotNodeEntity> AllNodes;

        /// <summary>
        /// 脏节点集合
        /// </summary>
        public HashSet<RedDotNodeEntity> DirtyNodes;

        /// <summary>
        /// 临时脏节点集合
        /// </summary>
        public List<RedDotNodeEntity> TempDirtyNodes;

        /// <summary>
        /// 路径分隔字符
        /// </summary>
        public char SplitChar;

        /// <summary>
        /// 缓存的StringBuild
        /// </summary>
        public StringBuilder CachedSb;

        /// <summary>
        /// 红点根节点
        /// </summary>
        public RedDotNodeEntity RootNode;
        
        

    }
}