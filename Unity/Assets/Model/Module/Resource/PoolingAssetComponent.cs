using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 池化资源组件
    /// </summary>
    public class PoolingAssetComponent: Entity
    {
        public Dictionary<string, AssetEntityPool> PathAssetEntityPools = new Dictionary<string, AssetEntityPool>();
    }

    public static class PoolingAssetComponentSystem
    {
        /// <summary>
        /// 同步获取资源实体
        /// </summary>
        public static AssetEntity GetAssetEntity(this PoolingAssetComponent self, string path)
        {
            if (self.PathAssetEntityPools.TryGetValue(path, out AssetEntityPool assetEntityPool))
            {
                return assetEntityPool.GetAssetEntity();
            }

            if (!AssetBundleHelper.GetBundlePrefabNameByPath(path, out string bundleName, out string prefabName))
            {
                Log.Error($"解析资源路径失败 资源路径:{path}");
                return null;
            }

            ResourcesComponent.Instance.LoadBundle(bundleName);
            GameObject obj = (GameObject) ResourcesComponent.Instance.GetAsset(bundleName, prefabName);
            AssetEntityPool newPool = EntityFactory.CreateWithParent<AssetEntityPool, GameObject, string, string>(self, obj, bundleName,prefabName);
            self.PathAssetEntityPools.Add(path, newPool);
            return newPool.GetAssetEntity();
        }

        /// <summary>
        /// 异步获取资源实体
        /// </summary>
        public static async ETTask<AssetEntity> GetAssetEntityAsync(this PoolingAssetComponent self, string path)
        {
            if (self.PathAssetEntityPools.TryGetValue(path, out AssetEntityPool assetEntityPool))
            {
                return assetEntityPool.GetAssetEntity();
            }

            if (!AssetBundleHelper.GetBundlePrefabNameByPath(path, out string bundleName, out string prefabName))
            {
                Log.Error($"解析资源路径失败 资源路径:{path}");
                return null;
            }

            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            GameObject obj = (GameObject) ResourcesComponent.Instance.GetAsset(bundleName, prefabName);
            AssetEntityPool newPool = EntityFactory.CreateWithParent<AssetEntityPool, GameObject, string, string>(self, obj, bundleName, prefabName);
            self.PathAssetEntityPools.Add(path, newPool);
            return newPool.GetAssetEntity();
        }
    }
}