using System;
using System.Collections.Generic;

namespace ET
{
    public static class SceneEntityHelper
    {
        public const string SceneDataDir = "Assets/Bundles/SceneConfigData";
        public const string SceneDataItemDirPre = "SceneData";

        public static string GetSceneDataFilePath(int sceneId, Type sceneDataType)
        {
            string path = $"{SceneDataDir}/SceneData{sceneId.ToString()}/{sceneDataType.Name}.bytes";
            return path;
        }

        public static string GetSceneDataBundleName(int sceneId)
        {
            return $"{SceneDataDir}/SceneData{sceneId.ToString()}";
        }

        public static string GetSceneDataKey(int sceneId, Type sceneDataType)
        {
            return $"{sceneId.ToString()}/{sceneDataType.Name}";
        }

        public static async ETTask LoadSceneEntitiesByType(Entity domain, SceneEntityManifest manifest,Type sceneDataType)
        {
            //wenchao 补全所有sceneEntity加载
            int sceneId = manifest.SceneId;
            if (sceneDataType == typeof(CharacterInfo))
            {
                
            }else if (sceneDataType == typeof(InteractionInfo))
            {
                
            }else if (sceneDataType == typeof(PickableInfo))
            {
                
            }else if (sceneDataType == typeof(TriggerBoxInfo))
            {
                
            }else if (sceneDataType == typeof(BuildingInfo))
            {
                await BuildingInfoHelper.LoadBuildings(domain, manifest);
            }
        }
        
        
        public static void AddSceneEntiyToDic( SceneEntity sceneEntity)
        {
            AssetEntity assetEntity = sceneEntity.GetComponent<AssetEntity>();

            if (assetEntity == null)
            {
                Log.Error($"场景实体 缺少 AssetEntity组件 entityId:{sceneEntity.Id.ToString()}");
                return;
            }

            if (!SceneEntityComponent.Instance.SceneEntities.ContainsKey(assetEntity.Object.GetInstanceID()))
            {
                SceneEntityComponent.Instance.SceneEntities.Add(assetEntity.Object.GetInstanceID(), sceneEntity);
            }
            else
            {
                Log.Error($"重复添加GameObject EntityId:{sceneEntity.Id.ToString()} GobjInstanceId:{assetEntity.Object.GetInstanceID().ToString()} ");
            }
        }
    }
}