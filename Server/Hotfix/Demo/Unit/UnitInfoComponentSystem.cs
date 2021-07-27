namespace ET
{
    public class UnitInfoComponentAwakeSystem : AwakeSystem<UnitInfoComponent, long>
    {
        public override void Awake(UnitInfoComponent self, long PlayerId)
        {
            self.PlayerId = PlayerId;
        }
    }
}