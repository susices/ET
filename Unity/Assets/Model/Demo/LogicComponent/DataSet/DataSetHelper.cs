using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 数据集辅助类
    /// </summary>
    public static class DataSetHelper
    {
        /// <summary>
        /// 覆盖更新数据集
        /// </summary>
        public static void OverwriteUpdate<T>(DataSetComponent dataSetComponent, List<T> dataList) where T: class,IDataMessage
        {
            var type = typeof (T);
            var dataSet = dataSetComponent.DataSet;
            if (dataSet.ContainsKey(type))
            {
                dataSet[type].Clear();
            }
            else
            {
                dataSet.Add(type,new Dictionary<int, IDataMessage>());
            }

            if (dataList==null || dataList.Count==0)
            {
                return;
            }

            foreach (var data in dataList)
            {
                dataSet[type].Add(data.DataId, data);
            }
        }

        /// <summary>
        /// 差异更新数据集
        /// </summary>
        public static void DifferenceUpdate<T>(DataSetComponent dataSetComponent, List<T> dataList) where T: class,IDataMessage
        {
            if (dataList==null || dataList.Count==0)
            {
                return;
            }
            var type = typeof (T);
            var dataSet = dataSetComponent.DataSet;

            if (!dataSet.ContainsKey(type))
            {
                Log.Error(string.Format("不存在 {1} 数据集", nameof(T)));
                return;
            }

            var dataDic = dataSet[type];
            
            foreach (var data in dataList)
            {
                IDataMessage dataMessage;
                if(!dataDic.TryGetValue(data.DataId, out dataMessage))
                {
                    if(data.DataValue>0)
                        dataDic[data.DataId] = dataMessage = data;
                }
                else
                {
                    dataMessage.DataValue += data.DataValue;
                }
                
                if (dataMessage.DataValue <= 0)
                {
                    dataDic.Remove(data.DataId);
                }
            }
        }
    }
}