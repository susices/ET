using System;
using UnityEngine;

namespace ET
{
	[ActorMessageHandler]
	public class M2M_TrasferUnitRequestHandler : AMActorLocationRpcHandler<Scene, M2M_TrasferUnitRequest, M2M_TrasferUnitResponse>
	{
		protected override async ETTask Run(Scene scene, M2M_TrasferUnitRequest request, M2M_TrasferUnitResponse response, Action reply)
		{
			Unit unit = request.Unit;
			//更新unit坐标
			unit.Position = new Vector3(request.X, request.Y, request.Z);
			
			// 这里不需要注册location，因为unlock会更新位置
			
			//mailbox 没有被序列化 需要手动添加组件
			unit.AddComponent<MailBoxComponent>();
			
			// 这里会将unit加入事件系统
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
			response.Error = ErrorCode.ERR_Success;
			reply();
			await ETTask.CompletedTask;
		}
	}
}