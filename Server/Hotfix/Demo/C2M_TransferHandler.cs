﻿using System;
using System.Net;


namespace ET
{
	[ActorMessageHandler]
	public class C2M_TransferHandler : AMActorLocationRpcHandler<Unit, C2M_Transfer, M2C_Transfer>
	{
		protected override async ETTask Run(Unit unit, C2M_Transfer request, M2C_Transfer response, Action reply)
		{
			//广播移除传送的unit  
			M2C_RemoveUnits m2CRemoveUnits = new M2C_RemoveUnits();
			m2CRemoveUnits.UnitIds.Add(unit.Id);
			MessageHelper.Broadcast(unit, m2CRemoveUnits);
			
			long unitId = unit.Id;
			// 先在location锁住unit的地址
			await Game.Scene.GetComponent<LocationProxyComponent>().Lock(unitId, unit.InstanceId);

			// 删除unit,让其它进程发送过来的消息找不到actor，重发
			Game.EventSystem.Remove(unitId);
			
			long instanceId = unit.InstanceId;
			int mapIndex = request.MapIndex;
			string mapName = MapNavMeshConfigCategory.Instance.Get(mapIndex).MapName;
			
			//获取传送map的 actorId
			long mapInstanceId = StartSceneConfigCategory.Instance.GetBySceneName(unit.DomainZone(), mapName).SceneId;

			// 只删除不disponse否则M2M_TrasferUnitRequest无法序列化Unit
			Game.Scene.GetComponent<UnitComponent>().RemoveNoDispose(unitId);

			//传送消息发给目标Map
			M2M_TrasferUnitResponse m2m_TrasferUnitResponse = (M2M_TrasferUnitResponse)await ActorMessageSenderComponent.Instance.Call
					(mapInstanceId,new M2M_TrasferUnitRequest() { Unit = unit });
			unit.Dispose();
			
			// 解锁unit的地址,并且更新unit的instanceId
			await Game.Scene.GetComponent<LocationProxyComponent>().UnLock(unitId, instanceId, m2m_TrasferUnitResponse.InstanceId);

			reply();
			await ETTask.CompletedTask;
		}
	}
}