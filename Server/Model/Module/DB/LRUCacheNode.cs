namespace ET
{
    public class LRUCacheNode
    {
        public LRUCacheNode Pre;

        public LRUCacheNode Next;

        public long PlayerId;

        public void Clear()
        {
            this.Pre = null;
            this.Next = null;
            this.PlayerId = 0;
        }
    }
}