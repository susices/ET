using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 资源实体池
    /// </summary>
    public class AssetEntityPool : Entity
    {
        public Queue<GameObject> Pool = new Queue<GameObject>();

        public GameObject GameObjectRes;
        
        public string AssetPath;

        public int CachePoolMillSeconds;
        
        public string BundleName;

        public int RefCount { get; set; }

        public long LastUseObjectTime;
    }
    
}