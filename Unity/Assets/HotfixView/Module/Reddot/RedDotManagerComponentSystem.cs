using System.Collections.Generic;
using System.Text;

namespace ET.Module.Reddot
{
    public class RedDotManagerComponentAwakeSystem : AwakeSystem<RedDotManagerComponent>
    {
        public override void Awake(RedDotManagerComponent self)
        {
            RedDotManagerComponent.Instance = self;
            self.AllNodes = new Dictionary<string, RedDotNodeEntity>();
            self.SplitChar = '/';
            self.RootNode = EntityFactory.Create<RedDotNodeEntity, string>(self.Domain, "Root");
            self.DirtyNodes = new HashSet<RedDotNodeEntity>();
            self.TempDirtyNodes = new List<RedDotNodeEntity>();
            self.CachedSb = new StringBuilder();
        }
    }

    public static class RedDotManagerComponentSystem
    {
        
    }
}