using System.Collections.Generic;

namespace ET.Module.Reddot
{
    public class RedDotManagerComponentAwakeSystem : AwakeSystem<RedDotManagerComponent>
    {
        public override void Awake(RedDotManagerComponent self)
        {
            RedDotManagerComponent.Instance = self;
            self.AllNodes = new Dictionary<string, RedDotNodeEntity>();
        }
    }

    public static class RedDotManagerComponentSystem
    {
        
    }
}