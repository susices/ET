using UnityEngine;
using UnityEngine.Rendering;

namespace ET
{
    public class AppStart_Init: AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();

            // 加载配置
            Game.Scene.AddComponent<ResourcesComponent>();
            
            await ResourcesComponent.Instance.LoadBundleAsync(AssetBundleHelper.ConfigDirPath);
            Game.Scene.AddComponent<ConfigComponent>();
            ConfigComponent.GetAllConfigBytes = LoadConfigHelper.LoadAllConfigBytes;
            await ConfigComponent.Instance.LoadAsync();
            ResourcesComponent.Instance.UnloadBundle(AssetBundleHelper.ConfigDirPath);
            
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            
            Game.Scene.AddComponent<NetThreadComponent>();

            Game.Scene.AddComponent<ZoneSceneManagerComponent>();
            
            Game.Scene.AddComponent<GlobalComponent>();

            Game.Scene.AddComponent<AIDispatcherComponent>();

            Game.Scene.AddComponent<SceneEntityConfigDataComponent>();

            Game.Scene.AddComponent<DataUpdateComponent>();

            Game.Scene.AddComponent<BuffActionDispatcher>();

            Game.Scene.AddComponent<SceneComponent>();
            
            // wenchao 修改load unit
            ResourcesComponent.Instance.LoadBundle("assets/bundles/unit");

            Scene zoneScene = await SceneFactory.CreateZoneScene(1, "Process");
            
            await Game.EventSystem.Publish(new EventType.AppStartInitFinish() { ZoneScene = zoneScene });

            Application.targetFrameRate = FrameworkConfigVar.DefaultFrameRate.IntVar();
        }
    }
}
