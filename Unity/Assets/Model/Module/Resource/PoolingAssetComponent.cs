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

    public static class PoolingAssetComponentSystem
    {
        /// <summary>
        /// 同步获取资源实体
        /// </summary>
        public static AssetEntity GetAssetEntity(this PoolingAssetComponent self, int AssetPathIndex, Transform parent = null)
        {
            var path = AssetPathIndex.LocalizedAssetPath();
            if (self.PathAssetEntityPools.TryGetValue(path, out AssetEntityPool assetEntityPool))
            {
                return assetEntityPool.GetAssetEntity(parent);
            }

            if (!AssetBundleHelper.GetBundlePrefabNameByPath(path, out string bundleName, out string prefabName))
            {
                Log.Error($"解析资源路径失败 资源路径:{path}");
                return null;
            }
            
            ResourcesComponent.Instance.LoadBundle(bundleName);
            GameObject obj = (GameObject) ResourcesComponent.Instance.GetAsset(bundleName, prefabName);
            AssetEntityPool newPool = EntityFactory.CreateWithParent<AssetEntityPool, GameObject, string, int>(self, obj, bundleName,AssetPathIndex);
            self.PathAssetEntityPools.Add(path, newPool);
            return newPool.GetAssetEntity(parent);
        }

        /// <summary>
        /// 异步获取资源实体
        /// </summary>
        public static async ETTask<AssetEntity> GetAssetEntityAsync(this PoolingAssetComponent self, int AssetPathIndex, Transform parent = null)
        {
            var path = AssetPathIndex.LocalizedAssetPath();
            if (self.PathAssetEntityPools.TryGetValue(path, out AssetEntityPool assetEntityPool))
            {
                return assetEntityPool.GetAssetEntity(parent);
            }

            if (!AssetBundleHelper.GetBundlePrefabNameByPath(path, out string bundleName, out string prefabName))
            {
                Log.Error($"解析资源路径失败 资源路径:{path}");
                return null;
            }

            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            GameObject obj = (GameObject) ResourcesComponent.Instance.GetAsset(bundleName, prefabName);
            AssetEntityPool newPool = EntityFactory.CreateWithParent<AssetEntityPool, GameObject, string, int>(self, obj, bundleName, AssetPathIndex);
            self.PathAssetEntityPools.Add(path, newPool);
            return newPool.GetAssetEntity(parent);
        }
    }
    
    public class PoolingAssetComponentAwakeSystem: AwakeSystem<PoolingAssetComponent>
    {
        public override void Awake(PoolingAssetComponent self)
        {
            PoolingAssetComponent.Instance = self;
            var obj = new GameObject();
            obj.name = "AssetPool";
            obj.transform.parent = GameObject.Find("/Global").transform;
            self.AssetPoolTransform = obj.transform;

        }
    }
}