using System;
using System.Collections.Generic;

namespace ET
{
    public class DataUpdateComponent: Entity
    {
        public Dictionary<DataType, List<Func<long,int,ETTask>>> dataUpdateEvents = new Dictionary<DataType, List<Func<long,int,ETTask>>>();
        public static DataUpdateComponent Instance { get; set; }
    }
    
}