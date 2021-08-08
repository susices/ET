

namespace ET
{
	[ActorMessageHandler]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
	{
		protected override async ETTask Run(Unit unit, G2M_SessionDisconnect message)
		{
			long unitId = unit.Id;
			Log.Debug($"unitid: {unitId.ToString()} playerId:{unit.GetComponent<UnitInfoComponent>().PlayerId.ToString()}已断线");
			var unitcomponent = unit.GetParent<UnitComponent>();
			unitcomponent.Remove(unitId);
			var msg = new M2C_RemoveUnits();
			msg.UnitIds.Add(unitId);
			MessageHelper.Broadcast(unitcomponent.GetAll(),msg);
			await ETTask.CompletedTask;
		}
	}
}