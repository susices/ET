using System;

namespace ET
{
    public class C2G_OffLineHandler:AMRpcHandler<C2G_OffLine,G2C_OffLine>
    {
        protected override async ETTask Run(Session session, C2G_OffLine request, G2C_OffLine response, Action reply)
        {
            
            reply();
            await ETTask.CompletedTask;
        }
    }
}