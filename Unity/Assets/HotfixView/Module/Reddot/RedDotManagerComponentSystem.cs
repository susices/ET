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

    public static class RedDotManagerComponentSystem
    {
        public static RedDotNodeEntity AddListener(this RedDotManagerComponent self, string path)
        {
            return null;
        }

        public static void RemoveListener(this RedDotManagerComponent self, string path)
        {
            
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