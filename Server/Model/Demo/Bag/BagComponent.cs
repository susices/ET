using System.Collections.Generic;

namespace ET
{
    public class BagComponent:Entity
    {
        public long PlayerId;
        public List<BagItem> BagItems = new List<BagItem>();
    }
}