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
                        var sceneTrans =  sceneRoot.Find(sceneId.ToString());
                        if (sceneTrans==null)
                        {
                            var sceneObj = new GameObject(sceneId.ToString());
                            sceneObj.transform.SetParent(sceneRoot);
                            sceneTrans = sceneObj.transform;
                        }
                        var asset = AssetDatabase.LoadAssetAtPath<GameObject>(buildingInfo.path);
                        GameObject assetObj =  PrefabUtility.InstantiatePrefab(asset, parent:sceneTrans) as GameObject;
                        assetObj.transform.position = buildInfo.GetPosition();
                        assetObj.transform.localScale = buildInfo.GetScale();
                        assetObj.transform.rotation = buildInfo.GetRotation();
                        break;
                }
            }
        }
    }
}

