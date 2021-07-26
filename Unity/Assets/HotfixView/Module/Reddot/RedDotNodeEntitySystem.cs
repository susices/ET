using System;
using System.Collections.Generic;

namespace ET
{
    public class RedDotNodeEntityAwakeSystemA : AwakeSystem<RedDotNodeEntity,string>
    {
        public override void Awake(RedDotNodeEntity self, string name)
        {
            self.Name = name;
            self.NodeValue = 0;
        }
    }
    
    public class RedDotNodeEntityAwakeSystemB : AwakeSystem<RedDotNodeEntity,string,RedDotNodeEntity>
    {
        public override void Awake(RedDotNodeEntity self, string name,RedDotNodeEntity parentNode)
        {
            self.Name = name;
            self.NodeValue = 0;
            self.ParentNode = parentNode;
        }
    }
    
    public class RedDotNodeEntityDestroySystem : DestroySystem<RedDotNodeEntity>
    {
        public override void Destroy(RedDotNodeEntity self)
        {
            foreach (var redDotUI in self.RedDotUIEntities.Values)
            {
                redDotUI.Dispose();
            }
        }
    }

    public static class RedDotNodeEntitySystem
    {
        public static RedDotNodeEntity GetOrAddChild(this RedDotNodeEntity self, RangeString key)
        {
            RedDotNodeEntity child = self.GetChild(key);
            if (child==null)
            {
                child = self.AddChild(key);
            }
            return child;
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
            if(self.ChildrenNodes.ContainsKey(key))
            {
                throw new Exception($"子节点添加失败，不允许重复添加：{self.GetFullPath()}");
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

        public static void RemoveAllChild(this RedDotNodeEntity self)
        {
            if (self.ChildrenNodes == null || self.ChildrenNodes.Count == 0)
            {
                return;
            }
            
            self.ChildrenNodes.Clear();
            RedDotManagerComponent.Instance.MarkDirtyNode(self);
            EventSystem.Instance.Publish(new EventType.ReddotNodeNumChange()).Coroutine();
        }

        private static void InternalChangeValue(this RedDotNodeEntity self, int newValue)
        {
            if (self.NodeValue == newValue)
            {
                return;
            }

            self.NodeValue = newValue;
            EventSystem.Instance.Publish(new EventType.ReddotNodeValueChange(){ReddotNodePath = self.GetFullPath(), NewValue = newValue}).Coroutine();
            RedDotManagerComponent.Instance.MarkDirtyNode(self.ParentNode);
        }

        public static void ChangeValue(this RedDotNodeEntity self, int newValue)
        {
            if (self.ChildrenNodes != null && self.ChildrenNodes.Count != 0)
            {
                throw new Exception($"不允许直接改变非叶子节点的值：{self.GetFullPath()}");
            }

            self.InternalChangeValue(newValue);
        }

        public static void ChangeDirtyNodeValue(this RedDotNodeEntity self)
        {
            int sum = 0;

            if (self.ChildrenNodes != null && self.ChildrenNodes.Count != 0)
            {
                foreach (KeyValuePair<RangeString, RedDotNodeEntity> child in self.ChildrenNodes)
                {
                    sum += child.Value.NodeValue;
                }
            }

            self.InternalChangeValue(sum);
        }

        public static string GetFullPath(this RedDotNodeEntity self)
        {
            if (string.IsNullOrEmpty(self.FullPath))
            {
                if (self.ParentNode == null || self.ParentNode == RedDotManagerComponent.Instance.RootNode)
                {
                    self.FullPath = self.Name;
                }
                else
                {
                    self.FullPath = $"{self.ParentNode.GetFullPath()}{RedDotManagerComponent.Instance.SplitChar.ToString()}{self.Name}";
                }
            }
            return self.FullPath;
        }

        public static void RegisterRedDotUIComponent(this RedDotNodeEntity self, Entity reddotUIComponent)
        {
            if (self.RedDotUIEntities.ContainsKey(reddotUIComponent.Id))
            {
                Log.Error("ReddotUIComponent Register Error!");
                return;
            }
            self.RedDotUIEntities.Add(reddotUIComponent.Id, reddotUIComponent);
        }

        public static void UnRegisterRedDotUIComponent(this RedDotNodeEntity self, Entity reddotUIComponent)
        {
            if (!self.RedDotUIEntities.Remove(reddotUIComponent.Id))
            {
                Log.Error("ReddotUIComponent UnRegister Error!");
            }
        }
    }
}