namespace ET
{
    public class UITipsDataDestroySystem : DestroySystem<UITipsData>
    {
        public override void Destroy(UITipsData self)
        {
            self.ContextText = null;
            self.ConfirmText = null;
            self.CancelText = null;
        }
    }

    public static class UITipsDataSystem
    {
        
    }
}