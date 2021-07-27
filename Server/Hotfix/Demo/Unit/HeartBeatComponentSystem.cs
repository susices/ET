namespace ET
{
    public class HeartBeatComponentAwakeSystem:AwakeSystem<HeartBeatComponent>
    {
        public override void Awake(HeartBeatComponent self)
        {
            self.LastPingTime = TimeHelper.ServerNow();
            self.DisconnectDeltaTime = 4000;
            self.CheckDeltaTime = 2000;
            self.LastCheckTime = TimeHelper.ServerNow();
        }
    }
    
    public class HeartBeatComponentUpdateSystem:UpdateSystem<HeartBeatComponent>
    {
        public override void Update(HeartBeatComponent self)
        {
            if (TimeHelper.ServerNow() - self.LastCheckTime< self.CheckDeltaTime)
            {
                return;
            }

            if ((TimeHelper.ServerNow()-self.LastPingTime)>=self.DisconnectDeltaTime)
            {
                Log.Debug($"断线 PlayerId:{self.GetParent<Session>().GetComponent<SessionPlayerComponent>().Player.Id.ToString()}");
                self.Dispose();
            }
            self.LastCheckTime = TimeHelper.ServerNow();
        }
    }
    
    public class HeartBeatComponentDestroySystem:DestroySystem<HeartBeatComponent>
    {
        public override void Destroy(HeartBeatComponent self)
        {
            self.GetParent<Session>().Dispose();
        }
    }
}