namespace ET
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> CreateZoneScene(int zone, string name)
        {
            Scene zoneScene = EntitySceneFactory.CreateScene(Game.IdGenerater.GenerateId(), zone, SceneType.Zone, name, Game.Scene);
            zoneScene.AddComponent<ZoneSceneFlagComponent>();
            zoneScene.AddComponent<NetKcpComponent>();
            zoneScene.AddComponent<ConsoleComponent>();
            zoneScene.AddComponent<UnitComponent>();
            //zoneScene.AddComponent<AIComponent, int>(1);
            zoneScene.AddComponent<TransferComponent>();
            // UI层的初始化
            zoneScene.AddComponent<UIEventComponent>();
            zoneScene.AddComponent<UIPanelComponent>();
            
            zoneScene.AddComponent<SceneEntityComponent>();
            await Game.EventSystem.Publish(new EventType.AfterCreateZoneScene() {ZoneScene = zoneScene});
            return zoneScene;
        }
    }
}