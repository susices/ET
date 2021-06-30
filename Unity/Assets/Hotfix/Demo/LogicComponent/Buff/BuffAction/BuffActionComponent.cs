using System.Collections.Generic;

namespace ET
{
    public class BuffActionComponent : Entity
    {
        public static BuffActionComponent Instance;

        public Dictionary<int, ABuffAction> idBuffActions = new Dictionary<int, ABuffAction>();
    }
}