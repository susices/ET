using System.Collections.Generic;
using System.Linq;

namespace ET
{
    /// <summary>
    /// Buff管理器组件系统
    /// </summary>
    public static class BuffManagerComponentSystem
    {
        /// <summary>
        /// 添加Buff实体
        /// </summary>
        public static bool AddBuffEntity(this BuffManaerComponent self, BuffEntity buffEntity)
        {
            //wenchao 添加Buff实体
            
            return true;
        }

        /// <summary>
        /// 移除Buff实体
        /// </summary>
        public static bool RemoveBuffEntity(this BuffManaerComponent self, BuffEntity buffEntity)
        {
            //wenchao 移除Buff实体
            
            return true;
        }
    }



    [ObjectSystem]
    public class BuffManagerAwakeSystem: AwakeSystem<BuffManaerComponent>
    {
        public override void Awake(BuffManaerComponent self)
        {
            self.parentEntity = self.Parent;
            self.idBuffEntities = new Dictionary<long, BuffEntity>();
        }
    }

    [ObjectSystem]
    public class BuffManagerUpdateSystem : UpdateSystem<BuffManaerComponent>
    {
        public override void Update(BuffManaerComponent self)
        {
            
        }
    }

    [ObjectSystem]
    public class BuffManagerDestroySystem: DestroySystem<BuffManaerComponent>
    {
        public override void Destroy(BuffManaerComponent self)
        {
            foreach (var buffEntity in self.idBuffEntities.Values)
            {
                buffEntity.Dispose();
            }
            
            self.idBuffEntities.Clear();
        }
    }
}