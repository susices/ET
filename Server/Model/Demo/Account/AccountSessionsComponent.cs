using System.Collections.Generic;

namespace ET
{
    public class AccountSessionsComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<long, long> AccounrSessionDictionary = new Dictionary<long, long>();
        
    }
}