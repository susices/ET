namespace ET
{
    public class GameObjectComponentSystem: DestroySystem<GameObjectComponent>
    {
        public override void Destroy(GameObjectComponent self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
        }
    }
}