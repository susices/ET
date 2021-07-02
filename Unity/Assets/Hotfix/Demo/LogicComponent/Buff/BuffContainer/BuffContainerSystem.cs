using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [ObjectSystem]
    public class BuffContainerAwakeSystem: AwakeSystem<BuffContainerComponent>
    {
        public override void Awake(BuffContainerComponent self)
        {
            self.idBuffEntities = new Dictionary<long, BuffEntity>();
        }
    }

    [ObjectSystem]
    public class BuffContainerUpdateSystem : UpdateSystem<BuffContainerComponent>
    {
        public override void Update(BuffContainerComponent self)
        {
            
        }
    }

    [ObjectSystem]
    public class BuffContainerDestroySystem: DestroySystem<BuffContainerComponent>
    {
        public override void Destroy(BuffContainerComponent self)
        {
            foreach (var buffEntity in self.idBuffEntities.Values)
            {
                buffEntity.Dispose();
            }
            
            self.idBuffEntities.Clear();
        }
    }
    
    
    /// <summary>
    /// Buff容器组件系统
    /// </summary>
    public static class BuffContainerSystem
    {
        /// <summary>
        /// 添加Buff到Buff容器
        /// </summary>
        public static void AddBuff(this BuffContainerComponent self, int buffConfigId, Entity sourceEntity)
        {
            BuffFactory.Create(self, sourceEntity,buffConfigId);
        }

        /// <summary>
        /// 从BuffManager移除Buff
        /// </summary>
        public static void RemoveBuff(this BuffContainerComponent self, long buffEntityId)
        {
            self.GetChild<BuffEntity>(buffEntityId).Dispose();
        }
    }
}