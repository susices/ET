namespace ET
{
	public class PlayerSystem : AwakeSystem<Player, string>
	{
		public override void Awake(Player self, string account)
		{
			self.Awake(account);
		}
	}

	public sealed class Player : Entity
	{
		public string Account { get; private set; }
		
		public long UnitId { get; set; }

		public long DBCacheId { get; set; }

		public void Awake(string account)
		{
			this.Account = account;
		}
	}
}