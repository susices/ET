namespace ET
{
    public static class TipsResultType
    {
        public const int Error = -1;
        public const int Confirm = 0;
        public const int Cancel = 1;
    }
    
    public class UITipsData: Entity,IAwake, IDestroy
    {
        public string ContextText;

        public string ConfirmText;

        public string CancelText;
    }
}