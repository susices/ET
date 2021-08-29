using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 场景实体配置数据组件
    /// </summary>
    public class SceneEntityConfigDataComponent:Entity
    {
        public static SceneEntityConfigDataComponent Instance { get; set; }
        
        public MultiDictionary<int,Type,SceneEntityManifest> SceneEntityManifests;
    }
}