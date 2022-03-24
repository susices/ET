using System.Collections.Generic;

namespace ET
{

    public interface IUnitCache
    {
        
    }
    
    public class UnitCache : Entity,IAwake,IDestroy
    {
        public string Key;

        public Dictionary<long, Entity> CacheComponentDic = new Dictionary<long, Entity>();

    }
}