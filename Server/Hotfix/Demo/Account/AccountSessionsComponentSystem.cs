namespace ET
{
    public class AccountSessionsComponentDestroySystem : DestroySystem<AccountSessionsComponent>
    {
        public override void Destroy(AccountSessionsComponent self)
        {
            self.AccounrSessionDictionary.Clear();
        }
    }
    
    public static class AccountSessionsComponentSystem
    {
        public static long Get(this AccountSessionsComponent self, long accountId)
        {
            if (!self.AccounrSessionDictionary.TryGetValue(accountId, out long instanceId))
            {
                return 0;
            }

            return instanceId;
        }

        public static void Add(this AccountSessionsComponent self, long accountId, long sesssionInstanceId)
        {
            if (self.AccounrSessionDictionary.ContainsKey(accountId))
            {
                self.AccounrSessionDictionary[accountId] = sesssionInstanceId;
                return;
            }
            self.AccounrSessionDictionary.Add(accountId, sesssionInstanceId);
        }

        public static void Remove(this AccountSessionsComponent self, long accountId)
        {
            if (self.AccounrSessionDictionary.ContainsKey(accountId))
            {
                self.AccounrSessionDictionary.Remove(accountId);
            }
        }
    }
}