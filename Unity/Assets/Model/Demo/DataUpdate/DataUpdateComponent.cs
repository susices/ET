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
        private Dictionary<DataUpdateType, List<Func<long,ETTask>>> dataUpdateEvents;
        public static DataUpdateComponent Instance { get; set; }

        public void Awake()
        {
            dataUpdateEvents = new Dictionary<DataUpdateType, List<Func<long,ETTask>>>();
        }

        /// <summary>
        /// 添加数据更新事件监听
        /// </summary>
        public void AddListener(DataUpdateType type, Func<long,ETTask> task)
        {
            if (!dataUpdateEvents.ContainsKey(type))
            {
                dataUpdateEvents.Add(type, new List<Func<long,ETTask>>());
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
        public void RemoveListener(DataUpdateType type, Func<long,ETTask> task)
        {
            if (dataUpdateEvents.ContainsKey(type))
            {
                dataUpdateEvents[type].Remove(task);
            }
        }

        public async ETTask Run(DataUpdateType dataUpdateType, long instanceId)
        {
            using (ListComponent<ETTask> ETTaskListComponent = ListComponent<ETTask>.Create())
            {
                var tcsList = ETTaskListComponent.List;
                var eventList = this.dataUpdateEvents[dataUpdateType];
                for (int i = eventList.Count; i >=0 ; i--)
                {
                    tcsList.Add(eventList[i].Invoke(instanceId)); ;
                }
                await ETTaskHelper.WaitAll(tcsList);
            }
        }
    }
}