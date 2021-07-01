using System.Collections.Generic;

namespace ET
{
    public class BuffActionDispatcher : Entity
    {
        public static BuffActionDispatcher Instance;

        public Dictionary<int, ABuffAction> idBuffActions = new Dictionary<int, ABuffAction>();
    }
}