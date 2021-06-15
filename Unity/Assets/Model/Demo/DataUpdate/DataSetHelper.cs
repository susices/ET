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
            var dataSet = dataSetComponent.DataSet;
            dataSet.Clear();

            if (dataList==null || dataList.Count==0)
            {
                return;
            }

            foreach (var data in dataList)
            {
                dataSet.Add(data.DataId, data);
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
            
            var dataSet = dataSetComponent.DataSet;
            foreach (var diffData in dataList)
            {
                IDataMessage oldData;
                if (dataSet.TryGetValue(diffData.DataId, out oldData))
                {
                    oldData.DataValue += diffData.DataValue;
                }
                else
                {
                    oldData = diffData;
                    dataSet.Add(oldData.DataId,oldData);
                }

                if (oldData.DataValue<=0)
                {
                    dataSet.Remove(oldData.DataId);
                }
            }
        }
    }
}