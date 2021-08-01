using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ET.EventType;
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
        public SortedList<int,IDataMessage> DataSet = new SortedList<int,IDataMessage>();
        /// <summary>
        /// 数据更新事件类型
        /// </summary>
        public DataType DataType;
    }
}