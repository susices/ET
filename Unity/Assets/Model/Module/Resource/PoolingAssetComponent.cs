using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 池化资源组件
    /// </summary>
    public class PoolingAssetComponent: Entity
    {
        public static PoolingAssetComponent Instance;
        public Dictionary<string, AssetEntityPool> PathAssetEntityPools = new Dictionary<string, AssetEntityPool>();
        public Transform AssetPoolTransform; 
    }
}