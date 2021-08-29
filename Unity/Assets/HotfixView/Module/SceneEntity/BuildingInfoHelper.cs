namespace ET
{
    public static class BuildingInfoHelper
    {
        public static async ETTask LoadBuildings(Entity domain, SceneEntityManifest manifest)
        {
            using var list = ListComponent<ETTask>.Create();
            foreach (var sceneEntityBuildInfo in manifest.list)
            {
                list.List.Add(LoadOneBuilding(domain,manifest.SceneId, sceneEntityBuildInfo));
            }
            await ETTaskHelper.WaitAll(list.List);
        }

        public static async ETTask LoadOneBuilding(Entity domain, int sceneId, SceneEntityBuildInfo sceneEntityBuildInfo)
        {
            if (sceneEntityBuildInfo.SceneEntityInfo==null)
            {
                return;
            }
            if (sceneEntityBuildInfo.SceneEntityInfo is BuildingInfo buildingInfo)
            {
                var sceneEntity = EntityFactory.Create<SceneEntity>(domain, isFromPool:true);
                var assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(buildingInfo.path, GlobalComponent.Instance.SceneEntity);
                assetEntity.Object.transform.position = sceneEntityBuildInfo.GetPosition();
                assetEntity.Object.transform.localScale = sceneEntityBuildInfo.GetScale();
                assetEntity.Object.transform.rotation = sceneEntityBuildInfo.GetRotation();
                sceneEntity.AddComponent(assetEntity);
                SceneEntityHelper.AddSceneEntiyToDic(sceneEntity);
            }
        }
    }
}