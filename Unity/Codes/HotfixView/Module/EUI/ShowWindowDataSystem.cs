namespace ET
{
    public class ShowWindowDataDestroySystem : DestroySystem<ShowWindowData>
    {
        public override void Destroy(ShowWindowData self)
        {
            self.contextData?.Dispose();
            self.contextData = null;
        }
    }

    public static class ShowWindowDataSystem
    {
        
    }
}