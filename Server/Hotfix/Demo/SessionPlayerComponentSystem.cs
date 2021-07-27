﻿

namespace ET
{
	public class SessionPlayerComponentDestroySystem : DestroySystem<SessionPlayerComponent>
	{
		public override void Destroy(SessionPlayerComponent self)
		{
			// 发送断线消息
			ActorLocationSenderComponent.Instance.Send(self.Player.UnitId, new G2M_SessionDisconnect());
			Log.Debug("发送断线消息");
			self.Domain.GetComponent<PlayerComponent>()?.Remove(self.Player.Id);
		}
	}
}
