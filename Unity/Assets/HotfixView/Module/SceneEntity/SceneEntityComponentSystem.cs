using System;
using System.Collections.Generic;

namespace ET
{
    public class SceneEntityComponentAwakeSystem: AwakeSystem<SceneEntityComponent>
    {
        public override void Awake(SceneEntityComponent self)
        {
            SceneEntityComponent.Instance = self;
            self.SceneEntities = new Dictionary<int, SceneEntity>();
            self.LoadedSceneIds = new HashSet<int>();
            var types = Game.EventSystem.GetTypes(typeof (SceneEntityInfoAttribute));
            self.SceneEntityInfoTypes = new List<Type>();
            foreach (Type type in types)
            {
                self.SceneEntityInfoTypes.Add(type);
            }
        }
    }

    public static class SceneEntityComponentSystem
    {
        public static async ETTask LoadSceneEntities(this SceneEntityComponent self, int sceneId)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.SceneEntityDic, 0))
            {
                if (self.LoadedSceneIds.Contains(sceneId))
                {
                    return;
                }
                
                await SceneEntityConfigDataComponent.Instance.LoadSceneEntityManifests(sceneId, self.SceneEntityInfoTypes);
                
                using var list = ListComponent<ETTask>.Create();
                foreach (Type sceneEntityInfoType in self.SceneEntityInfoTypes)
                {
                    SceneEntityManifest manifest = SceneEntityConfigDataComponent.Instance.GetSceneEntityManifest(sceneId, sceneEntityInfoType);
                    if (manifest == null)
                    {
                        continue;
                    }
                    list.List.Add(SceneEntityHelper.LoadSceneEntitiesByType(self.Domain,manifest, sceneEntityInfoType));
                }
                await ETTaskHelper.WaitAll(list.List);
                self.LoadedSceneIds.Add(sceneId);
            }
        }

        public static async ETTask UnLoadSceneEntities(this SceneEntityComponent self, int sceneId)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.SceneEntityDic, 0))
            {
                if (!self.LoadedSceneIds.Contains(sceneId))
                {
                    return;
                }

                foreach (SceneEntity sceneEntity in self.SceneEntities.Values)
                {
                    if (sceneEntity.SceneId == sceneId)
                    {
                        //卸载流程
                    }
                }
                self.LoadedSceneIds.Remove(sceneId);
                await ETTask.CompletedTask;
            }
        }
    }
}