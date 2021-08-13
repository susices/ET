namespace ET
{
    public class LRUCacheNode
    {
        public LRUCacheNode Pre;

        public LRUCacheNode Next;

        public long Id;

        public void Clear()
        {
            this.Pre = null;
            this.Next = null;
            this.Id = 0;
        }
    }
}