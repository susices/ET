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
            self.Awake();
        }
    }

    public class DataUpdateComponent: Entity
    {
        private Dictionary<DataType, List<Func<long,int,ETTask>>> dataUpdateEvents;
        public static DataUpdateComponent Instance { get; set; }

        public void Awake()
        {
            dataUpdateEvents = new Dictionary<DataType, List<Func<long,int,ETTask>>>();
        }

        /// <summary>
        /// 添加数据更新事件监听
        /// </summary>
        public void AddListener(DataType type, Func<long,int,ETTask> task)
        {
            if (!dataUpdateEvents.ContainsKey(type))
            {
                dataUpdateEvents.Add(type, new List<Func<long,int,ETTask>>());
            }

            if (dataUpdateEvents[type].Contains(task))
            {
                return;
            }

            dataUpdateEvents[type].Add(task);
        }

        /// <summary>
        /// 移除数据更新事件监听
        /// </summary>
        public void RemoveListener(DataType type, Func<long,int,ETTask> task)
        {
            if (dataUpdateEvents.ContainsKey(type))
            {
                dataUpdateEvents[type].Remove(task);
            }
        }

        /// <summary>
        /// 指定数据类型的更新事件广播
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="instanceId">数据更新组件Id</param>
        /// <param name="dataUpdateMode">数据更新模式</param>
        public async ETTask BroadCast(DataType dataType, long instanceId, int dataUpdateMode)
        {
            if (!this.dataUpdateEvents.ContainsKey(dataType))
            {
                return;
            }
            using (ListComponent<ETTask> ETTaskListComponent = ListComponent<ETTask>.Create())
            {
                var tcsList = ETTaskListComponent.List;
                var eventList = this.dataUpdateEvents[dataType];
                for (int i = eventList.Count-1; i >=0 ; i--)
                {
                    tcsList.Add(eventList[i].Invoke(instanceId,dataUpdateMode)); ;
                }
                await ETTaskHelper.WaitAll(tcsList);
            }
        }
    }
    
    
}