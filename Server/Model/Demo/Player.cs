namespace ET
{
	public class PlayerSystem : AwakeSystem<Player, string,long>
	{
		public override void Awake(Player self, string a, long playerId)
		{
			self.Awake(a,playerId);
		}
	}

	public sealed class Player : Entity
	{
		public string Account { get; private set; }

		public long PlayerId { get; private set; }

		public long UnitId { get; set; }

		public void Awake(string account, long playerId)
		{
			this.Account = account;
			this.PlayerId = playerId;
		}
	}
}