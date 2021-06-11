using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 数据集组件 配合逻辑组件同步更新数据
    /// </summary>
    public class DataSetComponent:Entity
    {
        /// <summary>
        /// 数据集
        /// </summary>
        public MultiDictionary<Type, int,IDataMessage> DataSet;
    }
}