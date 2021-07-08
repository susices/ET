namespace ET
{
    /// <summary>
    /// 场景切换辅助类
    /// </summary>
    public static class SceneChangeHelper
    {
        public static void UnloadSceneBundle(int sceneIndex)
        {
            var sceneConfig = SceneConfigCategory.Instance.Get(sceneIndex);
            if (!AssetBundleHelper.GetBundlePrefabNameByPath(sceneConfig.AssetPath.LocalizedAssetPath(), out var bundleName, out var prefabName))
            {
                Log.Error($"sceneIndex:{sceneIndex.ToString()}对应的场景Bundle：{sceneConfig.AssetPath.LocalizedAssetPath()}未找到！");
                return;
            }
            ResourcesComponent.Instance.UnloadBundle(bundleName);
        }

        public static async ETTask<bool> LoadSceneBundle(int sceneIndex)
        {
            var sceneConfig = SceneConfigCategory.Instance.Get(sceneIndex);
            if (!AssetBundleHelper.GetBundlePrefabNameByPath(sceneConfig.AssetPath.LocalizedAssetPath(), out var bundleName, out var prefabName))
            {
                Log.Error($"sceneIndex:{sceneIndex.ToString()}对应的场景Bundle：{sceneConfig.AssetPath.LocalizedAssetPath()}未找到！");
                return false;
            }
            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            return true;
        }
    }
}