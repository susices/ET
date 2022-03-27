using ET;

namespace ET
{
    public class SceneChangeFinish_UnloadOldCurrentScene: AEvent<EventType.SceneChangeFinish>
    {
        protected override async ETTask Run(EventType.SceneChangeFinish a)
        {
            if (a.oldCurrentScene==null)
            {
                return;
            }
            a.ZoneScene.GetComponent<SceneAssetComponent>().UnloadScene(a.oldCurrentScene);
            await ETTask.CompletedTask;
        }
    }
}