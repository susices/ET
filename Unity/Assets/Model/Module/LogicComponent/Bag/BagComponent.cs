using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 背包组件
    /// </summary>
    public class BagComponent : Entity
    {
        /// <summary>
        /// 背包物品数据集
        /// </summary>
        public SortedList<int,IDataMessage> BagItemDataSet;
    }
}