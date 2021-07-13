using UnityEngine;

namespace ET
{
    /// <summary>
    /// 资源实体
    /// </summary>
    public class AssetEntity : Entity
    {
        /// <summary>
        /// 实例化gameObject
        /// </summary>
        public GameObject Object { private set; get; }
        
        public string AssetPath { private set; get;}
        
        public void Awake(string assetPath,GameObject gameObject)
        {
            this.AssetPath = assetPath;
            this.Object = gameObject;
        }
        
        public void Destroy()
        {
            this.Object = null;
            this.AssetPath = null;
        }
    }
    
    
    
}