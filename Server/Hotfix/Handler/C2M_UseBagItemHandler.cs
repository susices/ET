using System;

namespace ET
{
    
    public class C2M_UseBagItemHandler : AMActorLocationRpcHandler<Unit, C2M_UseBagItem,M2C_UseBagItem>
    {
        protected override async ETTask Run(Unit unit, C2M_UseBagItem request, M2C_UseBagItem response, Action reply)
        {
            
            await ETTask.CompletedTask;
        }
    }
}