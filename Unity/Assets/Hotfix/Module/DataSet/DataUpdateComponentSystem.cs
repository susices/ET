using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class DataUpdateComponentAwakeSystem: AwakeSystem<DataUpdateComponent>
    {
        public override void Awake(DataUpdateComponent self)
        {
            DataUpdateComponent.Instance = self;
        }
    }

    public static class DataUpdateComponentSystem
    {
        
        /// <summary>
        /// 添加数据更新事件监听组件
        /// </summary>
        public static void AddListener(this DataUpdateComponent self,  DataType dataType, Entity component)
        {
            Dictionary<long, Entity> dic;
            if (!self.DataUpdateComponents.TryGetDic(dataType, out dic))
            {
                dic = new Dictionary<long, Entity>();
                self.DataUpdateComponents.Add(dataType, dic);
            }

            if (dic.ContainsKey(component.Id))
            {
                Log.Error("Can not add same Component in one datatype dic");
                return;
            }
            dic.Add(component.Id, component);
        }
        
        /// <summary>
        /// 移除数据更新监听组件
        /// </summary>
        public static void RemoveListener(this DataUpdateComponent self, DataType dataType, Entity component)
        {
            Dictionary<long, Entity> dic;
            if (!self.DataUpdateComponents.TryGetDic(dataType, out dic))
            {
                return;
            }

            if (!dic.Remove(component.Id))
            {
                Log.Error("Can not remove Component, not found");
            }
        }
    }
}