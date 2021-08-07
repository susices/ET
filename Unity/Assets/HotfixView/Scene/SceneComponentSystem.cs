namespace ET
{
    public static class SceneComponentSystem
    {
        public static async ETTask ChangeScene(this SceneComponent self, int sceneIndex)
        {
            if (self.activeSceneIndex!=null && sceneIndex==self.activeSceneIndex)
            {
                Log.Error($"尝试加载已加载的场景 sceneIndex: {sceneIndex.ToString()}!");
                return;
            }

            if (!await SceneChangeHelper.LoadSceneBundle(sceneIndex))
            {
                Log.Error($"加载场景资源失败 sceneIndex: {sceneIndex.ToString()}!");
                return;
            }
            
            using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
            {
                await sceneChangeComponent.ChangeSceneAsync(MapNavMeshConfigCategory.Instance.Get(sceneIndex).MapName);
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