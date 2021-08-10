namespace ET
{
    public enum FrameworkConfigVar
    {
        AssetPoolRecycleMillSeconds =1,
        
        DefaultFrameRate =2,
        
        LRUCapacity = 3,
    }

    public static class FrameworkConfigVarExtension
    {
        public static int IntVar(this FrameworkConfigVar self)
        {
            return FrameworkConfigCategory.Instance.Get((int) self).IntVar;
        }

        public static string StringVar(this FrameworkConfigVar self)
        {
            return FrameworkConfigCategory.Instance.Get((int) self).StringVar;
        }
    }
}