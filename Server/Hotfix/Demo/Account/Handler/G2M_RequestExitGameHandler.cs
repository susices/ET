using System;

namespace ET
{
    [ActorMessageHandler]
    public class G2M_RequestExitGameHandler : AMActorLocationRpcHandler<Unit, G2M_RequestExitGame,M2G_RequestExitGame>
    {
        protected override async ETTask Run(Unit unit, G2M_RequestExitGame request, M2G_RequestExitGame response, Action reply)
        {

            reply();
            await unit.RemoveLocation();
            unit.DomainScene().GetComponent<UnitComponent>().Remove(unit.Id);
            await ETTask.CompletedTask;
        }
    }
}