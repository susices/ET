namespace ET
{
    public static partial class TimerType
    {
        // 框架层0-1000，逻辑层的timer type 1000-9999
        public const int MoveTimer = 1001;
        public const int AITimer = 1002;
        public const int SessionAcceptTimeout = 1003;

        public const int AccountSessionCheckOutTime = 1004;

        public const int PlayerOfflineOutTime = 1005;

        public const int HideUITimeOut = 1006;

        public const int SaveChangeDBData = 1007;

        // 不能超过10000
    }
}