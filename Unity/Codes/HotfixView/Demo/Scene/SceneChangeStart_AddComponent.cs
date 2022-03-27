using BM;

namespace ET
{
    public class SceneChangeStart_AddComponent: AEvent<EventType.SceneChangeStart>
    {
        protected override async ETTask Run(EventType.SceneChangeStart args)
        {
            Scene currentScene = args.ZoneScene.CurrentScene();
            // 切换到map场景

            SceneChangeComponent sceneChangeComponent = null;
            try
            {
                var loadSceneHandler =  await AssetComponent.LoadSceneAsync($"Assets/Scenes/{currentScene.Name}.unity");
                args.ZoneScene.GetComponent<SceneAssetComponent>().AddloadSceneHandler(currentScene.Name, loadSceneHandler);
                sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>();
                {
                    await sceneChangeComponent.ChangeSceneAsync(currentScene.Name);
                }
            }
            finally
            {
                sceneChangeComponent?.Dispose();
            }
            
            currentScene.AddComponent<OperaComponent>();
        }
    }
}