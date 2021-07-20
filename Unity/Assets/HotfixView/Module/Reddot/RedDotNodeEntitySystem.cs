using System;
using System.Collections.Generic;

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
        public static RedDotNodeEntity GetOrAddChild(this RedDotNodeEntity self, RangeString key)
        {
            RedDotNodeEntity child = self.GetChild(key);
            if (child==null)
            {
                //child = 
            }

            return null;
        }

        public static RedDotNodeEntity GetChild(this RedDotNodeEntity self, RangeString key)
        {
            if (self.ChildrenNodes == null)
            {
                return null;
            }

            self.ChildrenNodes.TryGetValue(key, out RedDotNodeEntity child);
            return child;
        }

        public static RedDotNodeEntity AddChild(this RedDotNodeEntity self, RangeString key)
        {
            if (self.ChildrenNodes == null)
            {
                self.ChildrenNodes = new Dictionary<RangeString, RedDotNodeEntity>();
            }else if(self.ChildrenNodes.ContainsKey(key))
            {
                throw new Exception("子节点添加失败，不允许重复添加：" + self.FullPath);
            }

            RedDotNodeEntity child = EntityFactory.Create<RedDotNodeEntity, string, RedDotNodeEntity>(self.Domain, key.ToString(), self);
            self.ChildrenNodes.Add(key, child);
            EventSystem.Instance.Publish(new EventType.ReddotNodeNumChange()).Coroutine();
            return child;
        }

        public static bool RemoveChild(this RedDotNodeEntity self, RangeString key)
        {
            if (self.ChildrenNodes == null || self.ChildrenNodes.Count == 0)
            {
                return false;
            }

            RedDotNodeEntity child = self.GetChild(key);

            if (child != null)
            {
                //子节点被删除 需要进行一次父节点刷新
                RedDotManagerComponent.Instance.MarkDirtyNode(self);

                self.ChildrenNodes.Remove(key);

                EventSystem.Instance.Publish(new EventType.ReddotNodeNumChange()).Coroutine();

                return true;
            }

            return false;
        }
        
        
    }
}