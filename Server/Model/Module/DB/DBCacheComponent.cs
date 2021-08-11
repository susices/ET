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
        public Dictionary<long, Dictionary<Type, Entity>> UnitCaches = new Dictionary<long, Dictionary<Type, Entity>>();
        public Pool<Dictionary<Type, Entity>> UnitCachePool = new Pool<Dictionary<Type, Entity>>();
        public Pool<LRUCacheNode> CacheNodePool = new Pool<LRUCacheNode>();
    }
}