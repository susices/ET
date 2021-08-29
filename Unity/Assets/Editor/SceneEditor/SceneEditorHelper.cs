using System;
using System.Collections.Generic;
using ET;
using UnityEditor;
using UnityEngine;
using CharacterInfo = ET.CharacterInfo;
using Object = UnityEngine.Object;

namespace ETEditor
{
    public static class SceneEditorHelper
    {
        public const string SceneDataDir = "Assets/Bundles/SceneConfigData/";
        public const string SceneDataItemDirPre = "SceneData";

        public static Vector3 GetSceneViewCenterPos ()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            return sceneView.pivot;
        }

        public static void LoadManifestBySceneType(List<SceneEntityManifest> sceneEntityManifests, SceneEditType sceneEditType, Transform sceneRoot)
        {
            switch (sceneEditType)
            {
                case SceneEditType.All:
                    foreach (var manifest in sceneEntityManifests)
                    {
                        LoadSceneEntityByManifest(manifest,sceneRoot);
                    }
                    break;
                case SceneEditType.Dynamic:
                    break;
                case SceneEditType.Static:
                    break;
            }
        }

        public static Transform GetEntityParentTransform(Transform sceneRoot,int sceneId, Type entityType)
        {
            var sceneTrans =  sceneRoot.Find(sceneId.ToString());
            if (sceneTrans==null)
            {
                var sceneObj = new GameObject(sceneId.ToString());
                sceneObj.transform.SetParent(sceneRoot);
                sceneTrans = sceneObj.transform;
            }
            var entityParentTrans = sceneTrans.Find(entityType.Name);
            if (entityParentTrans==null)
            {
                var buildingObj = new GameObject(entityType.Name);
                buildingObj.transform.SetParent(sceneTrans);
                entityParentTrans = buildingObj.transform;
            }
            return entityParentTrans;
        }

        public static void LoadSceneEntityByManifest(SceneEntityManifest sceneEntityManifest, Transform sceneRoot)
        {
            if (sceneEntityManifest==null)
            {
                Debug.LogError("sceneEntityManifest is null!");
                return;
            }

            int sceneId = sceneEntityManifest.SceneId;
            
            
            foreach (var buildInfo in sceneEntityManifest.list)
            {
                var entityParentTrans = GetEntityParentTransform(sceneRoot, sceneId, buildInfo.SceneEntityInfo.GetType());
                switch (buildInfo.SceneEntityInfo)
                {
                    case CharacterInfo characterInfo:
                        break;
                    case InteractionInfo interactionInfo:
                        break;
                    case PickableInfo pickableInfo:
                        break;
                    case TriggerBoxInfo triggerBoxInfo:
                        break;
                    case BuildingInfo buildingInfo:
                        var asset = AssetDatabase.LoadAssetAtPath<GameObject>(buildingInfo.path);
                        GameObject assetObj =  PrefabUtility.InstantiatePrefab(asset, parent:entityParentTrans) as GameObject;
                        assetObj.transform.position = buildInfo.GetPosition();
                        assetObj.transform.localScale = buildInfo.GetScale();
                        assetObj.transform.rotation = buildInfo.GetRotation();
                        break;
                }
            }
        }

        public static void SaveSceneData(int sceneId,Type sceneDataType ,Transform sceneRoot)
        {
            var sceneDataRoot = sceneRoot.Find(sceneDataType.Name);
            if (sceneDataRoot==null)
            {
                Debug.LogError($"sceneId:{sceneId.ToString()} 不存在sceneDataType: {sceneDataType}");
                return;
            }

            int childCount = sceneDataRoot.transform.childCount;
            
            for (int i = 0; i < childCount; i++)
            {
                var childTransform =  sceneDataRoot.transform.GetChild(i);
                if (sceneDataType == typeof(CharacterInfo))
                {
                    
                }
            }
        }
    }
}

