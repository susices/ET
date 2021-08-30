using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ET;
using ProtoBuf;
using UnityEditor;
using UnityEngine;
using CharacterInfo = ET.CharacterInfo;


namespace ETEditor
{
    public static class SceneEditorHelper
    {
        public static Vector3 GetSceneViewCenterPos ()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            return sceneView.pivot;
        }

        public static void SaveSceneEntityManifestFile(SceneEntityManifest sceneEntityManifest, Type sceneDataType)
        {
            string SceneDataItemDir = $"{SceneEntityHelper.SceneDataDir}/SceneData{sceneEntityManifest.SceneId.ToString()}/";
            if (!Directory.Exists(SceneDataItemDir))
            {
                AssetDatabase.CreateFolder(SceneEntityHelper.SceneDataDir, $"SceneData{sceneEntityManifest.SceneId.ToString()}");
            }
            
            string path = Path.Combine(SceneDataItemDir, $"{sceneDataType.Name}.bytes");
            
            using (FileStream file = File.Create(path))
            {
                Serializer.Serialize(file, sceneEntityManifest);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void LoadManifestBySceneEditType(List<SceneEntityManifest> sceneEntityManifests, SceneEditType sceneEditType, Transform sceneRoot)
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

        public static void LoadAllSceneEntityTypeParentTransform(int sceneId, Transform sceneRoot)
        {
            var sceneDataTypes = ReflectionTools.GetImplementationsOf(typeof (ISceneEntityInfo));
            foreach (var sceneDataType in sceneDataTypes)
            {
                GetEntityParentTransform(sceneRoot, sceneId, sceneDataType);
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

        public static void SaveSceneDataBySceneEditType(int sceneId, SceneEditType sceneEditType, Transform sceneRoot)
        {
            var sceneDataTypes = ReflectionTools.GetImplementationsOf(typeof (ISceneEntityInfo));
            switch (sceneEditType)
            {
                case SceneEditType.All:
                    foreach (var sceneDataType in sceneDataTypes)
                    {
                        SaveSceneData(sceneId, sceneDataType, sceneRoot);
                    }
                    break;
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

            SceneEntityManifest sceneEntityManifest = new SceneEntityManifest();
            sceneEntityManifest.SceneId = sceneId;

            int childCount = sceneDataRoot.transform.childCount;
            
            for (int i = 0; i < childCount; i++)
            {
                var childTransform =  sceneDataRoot.transform.GetChild(i);
                SceneEntityBuildInfo sceneEntityBuildInfo = new SceneEntityBuildInfo();
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
                    BuildingInfo buildingInfo = new BuildingInfo();
                    sceneEntityBuildInfo.SceneEntityInfo = buildingInfo;
                    buildingInfo.path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(childTransform.gameObject);
                    Debug.Log($"Save buildingInfo name:{childTransform.name} path: {buildingInfo.path}");
                }

                var position = childTransform.position;
                sceneEntityBuildInfo.Position = new float[] { position.x, position.y, position.z };
                var localScale = childTransform.localScale;
                sceneEntityBuildInfo.Scale = new float[] { localScale.x, localScale.y, localScale.z };
                var rotation = childTransform.rotation;
                sceneEntityBuildInfo.Rotation = new float[] { rotation.x, rotation.y, rotation.z, rotation.w};
                sceneEntityManifest.list.Add(sceneEntityBuildInfo);
            }
            SaveSceneEntityManifestFile(sceneEntityManifest, sceneDataType);
        }
    }
}

