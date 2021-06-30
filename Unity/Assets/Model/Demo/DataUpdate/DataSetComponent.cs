using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ET.EventType;
namespace ET
{
    [ObjectSystem]
    public class DataSetComponentAwakeSystem : AwakeSystem<DataSetComponent,DataType>
    {
        public override void Awake(DataSetComponent self, DataType dataType)
        {
            self.DataSet = new Dictionary<int,IDataMessage>();
            self.DataType = dataType;
            self.DifferenceUpdateIndexList = new List<int>();
        }
    }
    
    
    public static class DataSetComponentSystem
    {
        public static void UpdateData<T>(this DataSetComponent self,int updateMode, List<T> dataList) where T: class,IDataMessage
        {
            if (updateMode == DataUpdateMode.Overwrite)
            {
                DataSetHelper.OverwriteUpdate(self, dataList);
                EventSystem.Instance.Publish(new DataUpdate()
                {
                    DataSetComponentId = self.InstanceId,
                    DataType = self.DataType,  
                    DataUpdateMode = DataUpdateMode.Overwrite
                }).Coroutine();
                
            }else if (updateMode == DataUpdateMode.Difference)
            {
                DataSetHelper.DifferenceUpdate(self,dataList);
                EventSystem.Instance.Publish(new DataUpdate()
                {
                    DataSetComponentId = self.InstanceId,
                    DataType = self.DataType,
                    DataUpdateMode = DataUpdateMode.Difference
                }).Coroutine();
            }
            else
            {
                Log.Error(string.Format("UpdateMode错误：{0}",updateMode.ToString()));
                return;
            }
            
            foreach (var data in self.DataSet.Values)
            {
                Log.Debug($"Type:{typeof(T)} id:{data.DataId.ToString()},value:{data.DataValue.ToString()}");
            }
        }
    }
    
    
    /// <summary>
    /// 数据集组件 配合逻辑组件同步更新数据
    /// </summary>
    public class DataSetComponent:Entity
    {
        /// <summary>
        /// 数据集
        /// </summary>
        public Dictionary<int,IDataMessage> DataSet;

        /// <summary>
        /// 差异更新索引列表
        /// </summary>
        public List<int> DifferenceUpdateIndexList;
        /// <summary>
        /// 数据更新事件类型
        /// </summary>
        public DataType DataType;
    }
}