using UnityEngine;

namespace ET
{
    public static class PoolingAssetHelper
    {
        public static AssetEntity GetAssetEntity(AssetEntityPool pool, string assetPath, Transform parent)
        {
            var assetEntity = EntityFactory.Create<AssetEntity, string, Transform>(pool.Domain, assetPath, parent, true);
            return assetEntity;
        }
    }
}