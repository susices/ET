using System;
using System.Collections.Generic;
using ET.EventType;

namespace ET
{
    [ObjectSystem]
    public class DataSetComponentAwakeSystem : AwakeSystem<DataSetComponent,DataUpdateType>
    {
        public override void Awake(DataSetComponent self, DataUpdateType dataUpdateType)
        {
            self.DataSet = new Dictionary<int,IDataMessage>();
            self.DataUpdateType = dataUpdateType;
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
                    ComponentId = self.InstanceId,
                    DataUpdateType = self.DataUpdateType
                }).Coroutine();
            }else if (updateMode == DataUpdateMode.Difference)
            {
                DataSetHelper.DifferenceUpdate(self,dataList);
                EventSystem.Instance.Publish(new DataUpdate()
                {
                    ComponentId = self.InstanceId,
                    DataUpdateType = self.DataUpdateType
                }).Coroutine();
            }
            else
            {
                Log.Error(string.Format("UpdateMode错误：{0}",updateMode.ToString()));
                return;
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
        /// 数据更新事件类型
        /// </summary>
        public DataUpdateType DataUpdateType;
    }
}