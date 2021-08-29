using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class SceneEntityConfigDataComponentAwakeSystem: AwakeSystem<SceneEntityConfigDataComponent>
    {
        public override void Awake(SceneEntityConfigDataComponent self)
        {
            SceneEntityConfigDataComponent.Instance = self;

            self.SceneEntityManifests = new MultiDictionary<int, Type, SceneEntityManifest>();
        }
    }

    public static class SceneEntityConfigDataComponentSystem
    {
        public static SceneEntityManifest GetSceneEntityManifest(this SceneEntityConfigDataComponent self, int sceneId, Type sceneDataType)
        {
            if (self.SceneEntityManifests.TryGetValue(sceneId, sceneDataType, out var manifest))
            {
                return manifest;
            }
            return null;
        }

        public static async ETTask LoadSceneEntityManifests(this SceneEntityConfigDataComponent self, int sceneId, List<Type> sceneDataTypes)
        {
            if (self.SceneEntityManifests.ContainsKey(sceneId))
            {
                return;
            }
            var bundleName = SceneEntityHelper.GetSceneDataBundleName(sceneId);
            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            using var list = ListComponent<ETTask>.Create();
            foreach (Type type in sceneDataTypes)
            {
                list.List.Add(self.LoadOneSceneEntityManifest(sceneId, type));
            }
            await ETTaskHelper.WaitAll(list.List);
            ResourcesComponent.Instance.UnloadBundle(bundleName);
        }

        public static async ETTask LoadOneSceneEntityManifest(this SceneEntityConfigDataComponent self, int sceneId, Type sceneDataType)
        {
            string configDataPath = SceneEntityHelper.GetSceneDataFilePath(sceneId, sceneDataType);
            TextAsset data = ResourcesComponent.Instance.GetAssetByPath<TextAsset>(configDataPath);
            SceneEntityManifest sceneEntityManifest =
                    ProtobufHelper.FromBytes(typeof (SceneEntityManifest), data.bytes, 0, data.bytes.Length) as SceneEntityManifest;
            if (sceneEntityManifest == null)
            {
                Log.Error($"读取SceneEntityManifest错误 SceneId:{sceneId.ToString()}  SceneDataType :{sceneDataType}");
                return;
            }
            self.SceneEntityManifests.Add(sceneId, sceneDataType, sceneEntityManifest);
            await ETTask.CompletedTask;
        }
    }
}