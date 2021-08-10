using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 数据库缓存组件
    /// </summary>
    public class DBCacheComponent:Entity
    {
        public int LRUCapacity;
        public Dictionary<long, LRUCacheNode> LruCacheNodes = new Dictionary<long, LRUCacheNode>();
        public LRUCacheNode HeadCacheNode;
        public LRUCacheNode TailCacheNode;
        public MultiDictionary<long, Type, Entity> UnitCaches = new MultiDictionary<long, Type, Entity>();
    }
}