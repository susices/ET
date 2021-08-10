using System;

namespace ET
{
    public class C2D_TestHandler:AMActorRpcHandler<Scene, C2D_Test,D2C_Test>
    {
        protected override async ETTask Run(Scene scene, C2D_Test request, D2C_Test response, Action reply)
        {
            Log.Info($"{scene.Name} 收到消息: {request.TestMsg}");
            reply();
            await ETTask.CompletedTask;
        }
    }
}