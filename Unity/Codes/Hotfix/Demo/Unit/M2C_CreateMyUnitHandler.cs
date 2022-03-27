﻿namespace ET
{
	[MessageHandler]
	public class M2C_CreateMyUnitHandler : AMHandler<M2C_CreateMyUnit>
	{
		protected override async ETTask Run(Session session, M2C_CreateMyUnit message)
		{
			// 通知场景切换协程继续往下走
			Log.Debug("before M2C_CreateMyUnitHandler");
			session.DomainScene().GetComponent<ObjectWait>().Notify(new WaitType.Wait_CreateMyUnit() {Message = message});
			Log.Debug("after M2C_CreateMyUnitHandler");
			await ETTask.CompletedTask;
		}
	}
}
