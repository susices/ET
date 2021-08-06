using System;


namespace ET
{
	[ActorMessageHandler]
	public class M2M_TrasferUnitRequestHandler : AMActorLocationRpcHandler<Scene, M2M_TrasferUnitRequest, M2M_TrasferUnitResponse>
	{
		protected override async ETTask Run(Scene scene, M2M_TrasferUnitRequest request, M2M_TrasferUnitResponse response, Action reply)
		{
			Unit unit = request.Unit;
			// 将unit加入事件系统
			// 这里不需要注册location，因为unlock会更新位置
			//unit.AddComponent<MailBoxComponent>();
			scene.GetComponent<UnitComponent>().Add(unit);
			
			//自己通知给周围人
			M2C_CreateUnits createUnits = new M2C_CreateUnits();
            			createUnits.Units.Add(UnitHelper.CreateUnitInfo(unit));
			MessageHelper.Broadcast(unit, createUnits);
			
			// 把周围的人通知给自己
			createUnits.Units.Clear();
			Unit[] units = scene.GetComponent<UnitComponent>().GetAll();
			foreach (Unit u in units)
			{
				createUnits.Units.Add(UnitHelper.CreateUnitInfo(u));
			}
			
			MessageHelper.SendActor(unit.GetComponent<UnitGateComponent>().GateSessionActorId, createUnits);
			
			response.InstanceId = unit.InstanceId;
			reply();
			await ETTask.CompletedTask;
		}
	}
}