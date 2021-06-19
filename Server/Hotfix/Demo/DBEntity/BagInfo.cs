using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 背包信息
    /// </summary>
    public class BagInfo : Entity
    {
        public long PlayerId;
        public List<BagItem> BagItems;
    }
}