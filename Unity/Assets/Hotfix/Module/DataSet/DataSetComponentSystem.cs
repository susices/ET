using System.Collections.Generic;
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
            DataUpdateComponent.Instance.DataSetComponents.Add(dataType, self);
        }
    }
    
    public class DataSetComponentDestroySystem : DestroySystem<DataSetComponent>
    {
        public override void Destroy(DataSetComponent self)
        {
            self.DataSet.Clear();
            self.DataType = DataType.None;
            DataUpdateComponent.Instance.DataSetComponents.Remove(self.DataType);
        }
    }

    public static class DataSetComponentSystem
    {
        public static async ETTask UpdateData<T>(this DataSetComponent self,int updateMode, List<T> dataList) where T: class,IDataMessage
        {
            if (updateMode == DataUpdateMode.Overwrite)
            {
                DataSetHelper.OverwriteUpdate(self, dataList);
                await EventSystem.Instance.Publish(new DataUpdate()
                {
                    DataType = self.DataType
                });

            }else if (updateMode == DataUpdateMode.Difference)
            {
                DataSetHelper.DifferenceUpdate(self,dataList);
                await EventSystem.Instance.Publish(new DataUpdate()
                {
                    DataType = self.DataType
                });
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
}