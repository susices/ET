using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 场景实体组件
    /// 管理运行时动态创建的场景实体
    /// </summary>
    public class SceneEntityComponent : Entity
    {
        public static SceneEntityComponent Instance;
        
        /// <summary>
        /// key为gameobject instanceId
        /// 的场景实体字典
        /// </summary>
        public Dictionary<int, SceneEntity> SceneEntities;

        /// <summary>
        /// 已加载的场景Id
        /// </summary>
        public HashSet<int> LoadedSceneIds;

        /// <summary>
        /// 场景是他信息类型
        /// </summary>
        public List<Type> SceneEntityInfoTypes;
    }
}