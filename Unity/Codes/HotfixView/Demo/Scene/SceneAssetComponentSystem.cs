using BM;

namespace ET
{
    
    public static class SceneAssetComponentSystem
    {
        public static void AddloadSceneHandler(this SceneAssetComponent self,string sceneName, LoadSceneHandler loadSceneHandler)
        {
            if (self.LoadSceneHandlers.ContainsKey(sceneName))
            {
                Log.Error($"重复添加loadSceneHandler ：{sceneName}");
                return;
            }
            self.LoadSceneHandlers.Add(sceneName, loadSceneHandler);
        }
        
        public static void UnloadScene(this SceneAssetComponent self, string sceneName)
        {
            if (self.LoadSceneHandlers.TryGetValue(sceneName, out var loadSceneHandler))
            {
                self.LoadSceneHandlers.Remove(sceneName);
                loadSceneHandler.UnLoad();
            }
        }
    }
}