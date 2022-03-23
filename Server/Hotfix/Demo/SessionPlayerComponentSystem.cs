

namespace ET
{
	public static class SessionPlayerComponentSystem
	{
		public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
		{
			public override void Destroy(SessionPlayerComponent self)
			{
				if (self.IsNeedKickPlayer)
				{
					Log.Debug("sessionPlayer释放 KickPlayer");
					Player player = self.Domain.GetComponent<PlayerComponent>().Get(self.AccountId);
					DisconnectHelper.KickPlayer(player).Coroutine();
				}
				else
				{
					Log.Debug("sessionPlayer释放 保留player");
				}
			}
		}

		public static Player GetMyPlayer(this SessionPlayerComponent self)
		{
			return self.Domain.GetComponent<PlayerComponent>().Get(self.AccountId);
		}
	}
}
