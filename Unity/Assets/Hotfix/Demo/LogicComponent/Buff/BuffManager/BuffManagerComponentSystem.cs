using System.Collections.Generic;
using System.Linq;

namespace ET
{
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
    
    
    /// <summary>
    /// Buff管理器组件系统
    /// </summary>
    public static class BuffManagerComponentSystem
    {
        /// <summary>
        /// 添加Buff到BuffManager
        /// </summary>
        public static void AddBuff(this BuffManaerComponent self, int buffConfigId, Entity sourceEntity)
        {
            BuffFactory.Create(self, sourceEntity,buffConfigId);
        }

        /// <summary>
        /// 从BuffManager移除Buff
        /// </summary>
        public static void RemoveBuff(this BuffManaerComponent self, long buffEntityId)
        {
            self.GetChild<BuffEntity>(buffEntityId).Dispose();
        }
    }
}