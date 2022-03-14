namespace ET
{
    public static class HotReloadHelper
    {
        /// <summary>
        /// 热重载逻辑层代码
        /// </summary>
        public static void ReloadCode()
        {
            CodeLoader.Instance.LoadLogic();
            Game.EventSystem.Add(CodeLoader.Instance.GetTypes());
            Game.EventSystem.Load();
            Log.Debug("hot reload success!");
        }

        public static void ReloadConfig()
        {
            ConfigComponent.Instance.Load();
        }
    }
}