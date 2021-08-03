namespace ET
{
    [BaseBuffAction(1)]
    public class TestAddBuffAction : IBuffAction
    {
        public void Run(BuffEntity buffEntity, int[] argsList)
        {
            foreach (var args  in argsList)
            {
                Log.Debug(args.ToString());
            }
        }
    }
    
    [BaseBuffAction(2)]
    public class TestTimeOutBuffAction : IBuffAction
    {
        public void Run(BuffEntity buffEntity, int[] argsList)
        {
            foreach (var args  in argsList)
            {
                Log.Debug(args.ToString());
            }
        }
    }
    
    [BaseBuffAction(3)]
    public class TestTickBuffAction : IBuffAction
    {
        public void Run(BuffEntity buffEntity, int[] argsList)
        {
            foreach (var args  in argsList)
            {
                Log.Debug(args.ToString());
            }
        }
    }
}