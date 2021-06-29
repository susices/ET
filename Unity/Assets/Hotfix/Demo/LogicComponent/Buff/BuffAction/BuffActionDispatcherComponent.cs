using System.Collections.Generic;

namespace ET
{
    public class BuffActionDispatcherComponent : Entity
    {
        public static BuffActionDispatcherComponent Instance;

        public Dictionary<string, ABuffAction> BuffActions = new Dictionary<string, ABuffAction>();
    }
}