using System;
using System.Collections.Generic;
using System.Text;
namespace ET
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
    
    public class RedDotManagerComponentUpdateSystem : UpdateSystem<RedDotManagerComponent>
    {
        public override void Update(RedDotManagerComponent self)
        {
            if (self.DirtyNodes.Count==0)
            {
                return;
            }
            
            self.TempDirtyNodes.Clear();
            foreach (RedDotNodeEntity node in self.DirtyNodes)
            {
                self.TempDirtyNodes.Add(node);
            }
            self.DirtyNodes.Clear();

            foreach (RedDotNodeEntity node in self.TempDirtyNodes)
            {
                node.ChangeDirtyNodeValue();
            }
        }
    }

    public static class RedDotManagerComponentSystem
    {
        public static void RegisterRedDotUIEntities(this RedDotManagerComponent self, string path, Entity reddotUI)
        {
            RedDotNodeEntity redDotNodeEntity = self.GetReddotNode(path);
            redDotNodeEntity.RegisterRedDotUIComponent(reddotUI);
        }

        public static void UnRegisterRedDotUIEntities(this RedDotManagerComponent self, string path, Entity reddotUI)
        {
            RedDotNodeEntity redDotNodeEntity = self.GetReddotNode(path);
            redDotNodeEntity.UnRegisterRedDotUIComponent(reddotUI);
        }

        public static void ChangeValue(this RedDotManagerComponent self, string path, int newValue)
        {
            
        }

        public static RedDotNodeEntity GetReddotNode(this RedDotManagerComponent self, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("路径不合法，不能为空");
            }

            if (self.AllNodes.TryGetValue(path, out RedDotNodeEntity node))
            {
                return node;
            }
            
            RedDotNodeEntity cur = self.RootNode;
            int length = path.Length;

            int startIndex = 0;

            for (int i = 0; i < length; i++)
            {
                if (path[i] == self.SplitChar)
                {
                    if (i == length - 1)
                    {
                        throw new Exception("路径不合法，不能以路径分隔符结尾：" + path);
                    }

                    int endIndex = i - 1;
                    if (endIndex < startIndex)
                    {
                        throw new Exception("路径不合法，不能存在连续的路径分隔符或以路径分隔符开头：" + path);
                    }

                    RedDotNodeEntity child = cur.GetOrAddChild(new RangeString(path,startIndex,endIndex));

                    //更新startIndex
                    startIndex = i + 1;

                    cur = child;
                }
            }

            //最后一个节点 直接用length - 1作为endIndex
            RedDotNodeEntity target = cur.GetOrAddChild(new RangeString(path, startIndex, length - 1));

            self.AllNodes.Add(path, target);

            return target;
        }


        public static bool RemoveTreeNode(this RedDotManagerComponent self, string path)
        {
            if (!self.AllNodes.ContainsKey(path))
            {
                return false;
            }

            RedDotNodeEntity node = self.GetReddotNode(path);
            self.AllNodes.Remove(path);
            return node.ParentNode.RemoveChild(new RangeString(node.Name, 0, node.Name.Length));
        }

        public static void RemoveAllTreeNode(this RedDotManagerComponent self)
        {
            self.RootNode.RemoveAllChild();
            self.AllNodes.Clear();
        }


        public static void MarkDirtyNode(this RedDotManagerComponent self, RedDotNodeEntity node)
        {
            if (node==null || node.Name == self.RootNode.Name)
            {
                return;
            }

            self.DirtyNodes.Add(node);
        }
        
        


    }
}