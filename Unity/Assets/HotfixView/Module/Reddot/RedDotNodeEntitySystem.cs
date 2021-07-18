namespace ET.Module.Reddot
{
    public class RedDotNodeEntityAwakeSystemA : AwakeSystem<RedDotNodeEntity,string>
    {
        public override void Awake(RedDotNodeEntity self, string name)
        {
            
        }
    }
    
    public class RedDotNodeEntityAwakeSystemB : AwakeSystem<RedDotNodeEntity,string,RedDotNodeEntity>
    {
        public override void Awake(RedDotNodeEntity self, string name,RedDotNodeEntity parentNode)
        {
            
        }
    }

    public static class RedDotNodeEntitySystem
    {
        
    }
}