namespace ET.Handler
{
    public class A2C_DisconnectHandler : AMHandler<A2C_Disconnect>
    {
        protected override async ETTask Run(Session session, A2C_Disconnect message)
        {
            Log.Error($"与服务器断开 ErrorCode: {message.Error.ToString()}");
            await ETTask.CompletedTask;
        }
    }
}