namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误
        
        // 110000以下的错误请看ErrorCore.cs
        
        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常

        public const int ERR_NetworkError = 200002;  // 网络错误
        public const int ERR_LoginInfoIsNullError = 200003;  //  登录信息为空错误
        public const int ERR_AccountNameFormError = 200004;  //  用户名格式错误
        public const int ERR_PasswordFormError = 200005;  //  密码格式错误
        public const int ERR_AccountInBlacklistError = 200006;  //  用户名黑名单错误
        public const int ERR_LoginPasswordError = 200007;  //  登陆密码错误
        public const int ERR_RequestRepeatedly = 200008;  //  请求重复错误
        public const int ERR_TokenError = 200009;  // Token错误
        public const int ERR_RoleNameIsNull = 200010;  // 角色名为空错误
        public const int ERR_RoleNameSame = 200011;  //  角色名重复
        public const int ERR_RoleNotExist = 200012;  //  角色不存在
        public const int ERR_RequestSceneTypeError = 200013; //  请求的服务器类型错误
        public const int ERR_ConnectGateKeyError = 200014;  // 连接Gate Key错误
        public const int ERR_OtherAccountLogin = 200015;  //  其他账户登录
        public const int ERR_SessionPlayerComponentError = 200016;  // sessionPlayer组件为空
        public const int ERR_NonePlayerError = 200017;  //  player不存在错误
        public const int ERR_PlayerSessionDisposeError = 200018;  //  player的session已被DIspose
        public const int ERR_SessionStateError = 200019;  //  session状态错误
        public const int ERR_EnterGameError = 200020;  //  进入逻辑服错误
        public const int ERR_ReEnterGameError = 200021;  //  二次登录错误
        public const int ERR_ReEnterGameError2 = 200022;  //  二次登录错误2

    }
}