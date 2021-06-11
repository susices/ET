using System;
using System.Collections.Generic;

namespace ET
{
    
    public class DataSetComponentAwakeSystem : AwakeSystem<DataSetComponent>
    {
        public override void Awake(DataSetComponent self)
        {
            self.DataSet = new MultiDictionary<Type, int,IDataMessage>();
        }
    }
    
    public static class DataSetComponentSystem
    {
        public static void UpdateData<T>(this DataSetComponent self,int updateMode, List<T> dataList) where T: class,IDataMessage
        {
            if (updateMode == DataUpdateMode.Overwrite)
            {
                DataSetHelper.OverwriteUpdate(self, dataList);
            }else if (updateMode == DataUpdateMode.Difference)
            {
               DataSetHelper.DifferenceUpdate(self,dataList);
            }
            else
            {
                Log.Error(string.Format("UpdateMode错误：{0}",updateMode.ToString()));
                return;
            }
        }
    }
}