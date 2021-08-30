namespace ET
{
    public class SceneEntityAwakeSystem:AwakeSystem<SceneEntity,int,int,SceneEntityType>
    {
        public override void Awake(SceneEntity self, int sceneId, int gameObjectInstanceId, SceneEntityType sceneEntityType)
        {
            self.SceneId = sceneId;
            self.SceneEntityType = sceneEntityType;
            self.GameObjectInstanceId = gameObjectInstanceId;
        }
    }

    public class SceneEntityDestorySystem:DestroySystem<SceneEntity>
    {
        public override void Destroy(SceneEntity self)
        {
            self.SceneId = 0;
            self.SceneEntityType = SceneEntityType.None;
        }
    }

    public static class SceneEntitySystem
    {
        
    }
}