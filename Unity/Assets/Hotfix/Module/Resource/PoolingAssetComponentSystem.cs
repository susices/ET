using UnityEngine;

namespace ET
{
    public static class PoolingAssetComponentSystem
    {
        /// <summary>
        /// 同步获取资源实体
        /// </summary>
        public static AssetEntity GetAssetEntity(this PoolingAssetComponent self, int AssetPathIndex, Transform parent = null)
        {
            var path = AssetPathIndex.LocalizedAssetPath();
            int cachePoolMillseconds = LocalizationComponent.Instance.CachePoolMillSeconds(AssetPathIndex);
            if (self.PathAssetEntityPools.TryGetValue(path, out AssetEntityPool assetEntityPool))
            {
                return PoolingAssetHelper.GetAssetEntity(assetEntityPool, path, parent);
            }

            if (!AssetBundleHelper.GetBundlePrefabNameByPath(path, out string bundleName, out string prefabName))
            {
                Log.Error($"解析资源路径失败 资源路径:{path}");
                return null;
            }
            
            ResourcesComponent.Instance.LoadBundle(bundleName);
            GameObject obj = (GameObject) ResourcesComponent.Instance.GetAsset(bundleName, prefabName);
            AssetEntityPool newPool = EntityFactory.CreateWithParent<AssetEntityPool, GameObject, string, string,int>(self, obj, bundleName,path,cachePoolMillseconds, true);
            self.PathAssetEntityPools.Add(path, newPool);
            return PoolingAssetHelper.GetAssetEntity(newPool, path, parent);
        }

        /// <summary>
        /// 通过多语言资源序号
        /// 异步获取资源实体
        /// </summary>
        public static async ETTask<AssetEntity> GetAssetEntityAsync(this PoolingAssetComponent self, int AssetPathIndex, Transform parent = null)
        {
            var path = AssetPathIndex.LocalizedAssetPath();
            int cachePoolMillseconds = LocalizationComponent.Instance.CachePoolMillSeconds(AssetPathIndex);
            if (self.PathAssetEntityPools.TryGetValue(path, out AssetEntityPool assetEntityPool))
            {
                return PoolingAssetHelper.GetAssetEntity(assetEntityPool, path, parent);
            }

            if (!AssetBundleHelper.GetBundlePrefabNameByPath(path, out string bundleName, out string prefabName))
            {
                Log.Error($"解析资源路径失败 资源路径:{path}");
                return null;
            }

            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            GameObject obj = (GameObject) ResourcesComponent.Instance.GetAsset(bundleName, prefabName);
            AssetEntityPool newPool = EntityFactory.CreateWithParent<AssetEntityPool, GameObject, string, string,int>(self, obj, bundleName, path, cachePoolMillseconds, true);
            self.PathAssetEntityPools.Add(path, newPool);
            return PoolingAssetHelper.GetAssetEntity(newPool, path, parent);
        }
        
        /// <summary>
        /// 通过资源路径
        /// 异步获取资源实体
        /// </summary>
        public static async ETTask<AssetEntity> GetAssetEntityAsync(this PoolingAssetComponent self, string AssetPath, Transform parent = null)
        {
            var path = AssetPath;
            int cachePoolMillseconds = FrameworkConfigVar.AssetPoolRecycleMillSeconds.IntVar();
            if (self.PathAssetEntityPools.TryGetValue(path, out AssetEntityPool assetEntityPool))
            {
                return PoolingAssetHelper.GetAssetEntity(assetEntityPool, path, parent);
            }

            if (!AssetBundleHelper.GetBundlePrefabNameByPath(path, out string bundleName, out string prefabName))
            {
                Log.Error($"解析资源路径失败 资源路径:{path}");
                return null;
            }

            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            GameObject obj = (GameObject) ResourcesComponent.Instance.GetAsset(bundleName, prefabName);
            AssetEntityPool newPool = EntityFactory.CreateWithParent<AssetEntityPool, GameObject, string, string,int>(self, obj, bundleName, path, cachePoolMillseconds, true);
            self.PathAssetEntityPools.Add(path, newPool);
            return PoolingAssetHelper.GetAssetEntity(newPool, path, parent);
        }

        /// <summary>
        /// 释放所有未使用的缓存资源
        /// 保留至指定的个数
        /// </summary>   
        public static void ReleaseAllUnUseCacheAsset(this PoolingAssetComponent self, int maxObjectCount = 0)
        {
            foreach (var assetEntityPool in self.PathAssetEntityPools.Values)
            {
                assetEntityPool.ReleaseUnUseObject(maxObjectCount);
            }
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