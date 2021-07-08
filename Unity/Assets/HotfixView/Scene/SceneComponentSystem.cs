namespace ET
{
    public static class SceneComponentSystem
    {
        public static async ETTask ChangeScene(this SceneComponent self, int sceneIndex)
        {
            if (sceneIndex==self.activeSceneIndex)
            {
                Log.Error($"尝试加载已加载的场景 sceneIndex: {sceneIndex.ToString()}!");
                return;
            }

            if (!await SceneChangeHelper.LoadSceneBundle(sceneIndex))
            {
                return;
            }
            
            using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
            {
                await sceneChangeComponent.ChangeSceneAsync(SceneConfigCategory.Instance.Get(sceneIndex).SceneName);
            }

            if (self.activeSceneIndex!=null)
            {
                SceneChangeHelper.UnloadSceneBundle(self.activeSceneIndex.Value);
            }
            
            self.activeSceneIndex = sceneIndex;
        }
    }
    
    public class SceneComponentAwakeSystem : AwakeSystem<SceneComponent>
    {
        public override void Awake(SceneComponent self)
        {
            self.activeSceneIndex = null;
        }
    }
}