using System.Collections.Generic;

namespace ET
{
    public class LRUComponentAwakeSystem:AwakeSystem<LRUComponent,int>
    {
        public override void Awake(LRUComponent self, int lruCapacity)
        {
            self.LRUCapacity = lruCapacity;
            self.LruCacheNodes = new Dictionary<long, LRUCacheNode>();
            self.CacheNodePool = new Pool<LRUCacheNode>();
            self.HeadCacheNode = null;
            self.TailCacheNode = null;
        }
    }
    
    public class LRUComponentDestroySystem:DestroySystem<LRUComponent>
    {
        public override void Destroy(LRUComponent self)
        {
            self.LRUCapacity = 0;
            self.LruCacheNodes = null;
            self.CacheNodePool = null;
            self.HeadCacheNode = null;
            self.TailCacheNode = null;
        }
    }

    public static class LRUComponentSystem
    {
        
    }
}