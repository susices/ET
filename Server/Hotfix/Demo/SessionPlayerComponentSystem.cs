

namespace ET
{
	public static class SessionPlayerComponentSystem
	{
		public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
		{
			public override void Destroy(SessionPlayerComponent self)
			{
				if (self.IsNeedKickPlayer &&self.PlayerInstanceId!=0)
				{
					Log.Debug("sessionPlayer释放 KickPlayer");
					Player player = Game.EventSystem.Get(self.PlayerInstanceId) as Player;
					DisconnectHelper.KickPlayer(player).Coroutine();
				}
				else
				{
					Log.Debug("sessionPlayer释放 保留player");
				}

				self.AccountId = 0;
				self.PlayerId = 0;
				self.PlayerInstanceId = 0;
				self.IsNeedKickPlayer = true;
			}
		}

		public static Player GetMyPlayer(this SessionPlayerComponent self)
		{
			return self.Domain.GetComponent<PlayerComponent>().Get(self.AccountId);
		}
	}
}
