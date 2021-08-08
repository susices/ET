
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace ET
{
	[MessageHandler]
	public class M2C_CreateUnitsHandler : AMHandler<M2C_CreateUnits>
	{
		protected override async ETVoid Run(Session session, M2C_CreateUnits message)
		{	
			UnitComponent unitComponent = session.Domain.GetComponent<UnitComponent>();
			
			foreach (UnitInfo unitInfo in message.Units)
			{
				if (unitComponent.Get(unitInfo.UnitId) != null)
				{// 处理传送unit 更新unit数据
					unitComponent.Get(unitInfo.UnitId).Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
					unitComponent.Get(unitInfo.UnitId).Rotation = new Quaternion(unitInfo.A, unitInfo.B, unitInfo.C, unitInfo.W);
					continue;
				}
				Unit unit = UnitFactory.Create(session.Domain, unitInfo);
			}

			await ETTask.CompletedTask;
		}
	}
}
