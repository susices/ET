﻿

namespace ET
{
	[ActorMessageHandler]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
	{
		protected override async ETTask Run(Unit unit, G2M_SessionDisconnect message)
		{
			Log.Debug($"unitid: {unit.Id} playerId:{unit.GetComponent<UnitInfoComponent>().PlayerId}已断线");
			unit.GetParent<UnitComponent>().Remove(unit.Id);
			await ETTask.CompletedTask;
		}
	}
}