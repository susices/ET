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
        /// <summary>
        /// 加载指定场景实体
        /// </summary>
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

        /// <summary>
        /// 卸载指定场景实体
        /// </summary>
        public static async ETTask UnLoadSceneEntities(this SceneEntityComponent self, int sceneId)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.SceneEntityDic, 0))
            {
                if (!self.LoadedSceneIds.Contains(sceneId))
                {
                    return;
                }

                using var list = ListComponent<int>.Create();
                
                foreach (SceneEntity sceneEntity in self.SceneEntities.Values)
                {
                    if (sceneEntity.SceneId == sceneId)
                    {
                        list.List.Add(sceneEntity.GameObjectInstanceId);
                        sceneEntity.Dispose();
                    }
                }

                foreach (var id in list.List)
                {
                    self.SceneEntities.Remove(id);
                }
                
                self.LoadedSceneIds.Remove(sceneId);
            }
        }

        /// <summary>
        /// 根据gameObject InstanceId
        /// 获取对应 SceneEntity类
        /// </summary>
        public static SceneEntity GetSceneEntity(this SceneEntityComponent self, int instanceId)
        {
            SceneEntity sceneEntity = null;
            self.SceneEntities.TryGetValue(instanceId, out sceneEntity);
            return sceneEntity;
        }
    }
}