using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class BuffContainerAwakeSystem: AwakeSystem<BuffContainerComponent>
    {
        public override void Awake(BuffContainerComponent self)
        {
            self.BuffState = BuffState.None;
        }
    }

    [ObjectSystem]
    public class BuffContainerDestroySystem: DestroySystem<BuffContainerComponent>
    {
        public override void Destroy(BuffContainerComponent self)
        {
            self.BuffState = BuffState.None;
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
        public static bool TryAddBuff(this BuffContainerComponent self, int buffConfigId, Entity sourceEntity)
        {
            //检测是否存在冲突状态
            if (CheckBuffConflict(self, buffConfigId, sourceEntity))
            {
                return false;
            }

            using var buffEntityList = ListComponent<BuffEntity>.Create();

            //检测是否含有该种Buff
            if (!CheckBuffByConfigId(self, buffConfigId, buffEntityList.List))
            {
                AddBuff(self, buffConfigId, sourceEntity);
                return true;
            }

            // 判断是否存在同一来源 buff
            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffConfigId);
            BuffEntity sameSourceBuffEntity = null;
            foreach (BuffEntity buffEntity in buffEntityList.List)
            {
                if (buffEntity.SourceEntity.Id == sourceEntity.Id)
                {
                    sameSourceBuffEntity = buffEntity;
                    break;
                }
            }
            //检测是否可以刷新该同来源Buff
            if (sameSourceBuffEntity != null)
            {
                if (!buffConfig.IsEnableRefresh)
                {
                    return false;
                }
                if (sameSourceBuffEntity.CurrentLayer < buffConfig.MaxLayerCount)
                {
                    sameSourceBuffEntity.CurrentLayer++;
                }
                BuffActionDispatcher.Instance.RunBuffRefreshAction(sameSourceBuffEntity);
                return true;
            }
            
            //检测是否可以添加新Buff
            if (buffEntityList.List.Count < buffConfig.MaxSourceCount)
            {
                AddBuff(self, buffConfigId, sourceEntity);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从BuffManager移除Buff
        /// </summary>
        public static bool TryRemoveBuff(this BuffContainerComponent self, long buffEntityId)
        {
            BuffEntity buffEntity = self.GetChild<BuffEntity>(buffEntityId);
            if (buffEntity == null)
            {
                return false;
            }
            BuffActionDispatcher.Instance.RunBuffRemoveAction(buffEntity);
            Log.Debug($"BuffRemoved BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            buffEntity.Dispose();
            return true;
        }

        private static void AddBuff(this BuffContainerComponent self, int buffConfigId, Entity sourceEntity)
        {
            var buffENtity =  BuffFactory.Create(self, sourceEntity, buffConfigId);
            self.BuffState = self.BuffState | buffENtity.State;
        }

        /// <summary>
        /// 检查Buff是否与容器Buff冲突
        /// </summary>
        /// <returns></returns>
        private static bool CheckBuffConflict(this BuffContainerComponent self, int buffConfigId, Entity sourceEntity)
        {
            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffConfigId);
            // 检测BuffState 冲突
            if (!BuffStateConfigCategory.Instance.Contain(buffConfig.State))
            {
                return false;
            }
            int[] conflictStates = BuffStateConfigCategory.Instance.Get(buffConfig.State).ConflictStates;
            if (conflictStates==null)
            {
                return false;
            }
            foreach (int conflictState in conflictStates)
            {
                if ((self.BuffState & (BuffState) conflictState) == (BuffState) conflictState)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查是否存在指定configId的Buff  并返回符合的buffEntity列表
        /// </summary>
        private static bool CheckBuffByConfigId(this BuffContainerComponent self, int buffConfigId, List<BuffEntity> buffEntities)
        {
            
            bool value = false;
            foreach (var child in self.Children)
            {
                if (child.Value is BuffEntity buffEntity && buffEntity.BuffConfigId == buffConfigId)
                {
                    buffEntities.Add(buffEntity);
                    value = true;
                }
            }
            return value;
        }
    }
}