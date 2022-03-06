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
                await AssetComponent.LoadSceneAsync($"Assets/Scenes/{currentScene.Name}.unity");
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