using System.Collections.Generic;

namespace ET
{
    public class DataUpdateComponent: Entity
    {
        public MultiDictionary<DataType, long, Entity> DataUpdateComponents = new MultiDictionary<DataType, long, Entity>();

        public Dictionary<DataType, DataSetComponent> DataSetComponents = new Dictionary<DataType, DataSetComponent>();
        
        public static DataUpdateComponent Instance { get; set; }
    }
}