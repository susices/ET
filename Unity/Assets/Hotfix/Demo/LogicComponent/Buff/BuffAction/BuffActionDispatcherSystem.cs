using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class BuffActionDispatcherLoadSystem: LoadSystem<BuffActionDispatcher>
    {
        public override void Load(BuffActionDispatcher self)
        {
            self.Load();
        }
    }

    [ObjectSystem]
    public class BuffActionDispatcherAwakeSystem: AwakeSystem<BuffActionDispatcher>
    {
        public override void Awake(BuffActionDispatcher self)
        {
            BuffActionDispatcher.Instance = self;
            self.Load();
        }
    }

    [ObjectSystem]
    public class BuffActionDispatcherDestroySystem: DestroySystem<BuffActionDispatcher>
    {
        public override void Destroy(BuffActionDispatcher self)
        {
            self.idBuffActions.Clear();
            BuffActionDispatcher.Instance = null;
        }
    }

    public static class BuffActionDispatcherSystem
    {
        public static void Load(this BuffActionDispatcher self)
        {
            self.idBuffActions.Clear();

            var types = Game.EventSystem.GetTypes(typeof (BaseBuffActionAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (BaseBuffActionAttribute), false);

                if (attrs.Length == 0)
                {
                    Log.Error($"{type.Name} do not has a BaseBuffActionAttribute!");
                    continue;
                }

                BaseBuffActionAttribute buffActionAttribute = attrs[0] as BaseBuffActionAttribute;
                ABuffAction aBuffAction = Activator.CreateInstance(type) as ABuffAction;
                if (aBuffAction == null)
                {
                    Log.Error($"{type.Name} is not a BuffAction!");
                    continue;
                }

                if (self.idBuffActions.ContainsKey(buffActionAttribute.Id))
                {
                    Type sameIdType = self.idBuffActions[buffActionAttribute.Id].GetType();
                    Log.Error($"{type.Name} has same id with {sameIdType.Name} : {buffActionAttribute.Id.ToString()}");
                    continue;
                }

                self.idBuffActions.Add(buffActionAttribute.Id, aBuffAction);
            }
        }

        /// <summary>
        /// 运行BuffAction方法
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        /// <param name="baseBuffActionId"></param>
        /// <param name="args"></param>
        public static void RunBuffAction(this BuffActionDispatcher self, BuffEntity buffEntity, int baseBuffActionId, int[] args)
        {
            if (!self.idBuffActions.TryGetValue(baseBuffActionId, out ABuffAction baseBuffAction))
            {
                Log.Error($"buffActionId {baseBuffActionId.ToString()} is not exist in idbuffActions!");
                return;
            }

            baseBuffAction.Run(buffEntity, args);
        }

        /// <summary>
        /// 运行BuffAction方法
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        /// <param name="buffActionIds"></param>
        public static void RunBuffActions(this BuffActionDispatcher self, BuffEntity buffEntity, int[] buffActionIds)
        {
            if (buffActionIds==null)
            {
                return;
            }
            
            foreach (int buffActionId in buffActionIds)
            {
                BuffActionConfig buffActionConfig = BuffActionConfigCategory.Instance.Get(buffActionId);
                int baseBuffActionId = buffActionConfig.BaseActionId;
                int[] args = buffActionConfig.actionArgs;
                self.RunBuffAction(buffEntity, baseBuffActionId, args);
            }
        }

        /// <summary>
        /// Buff添加后执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffAddAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffAddActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffAddActions;
            self.RunBuffActions(buffEntity, buffAddActionIds);
        }

        /// <summary>
        /// Buff移除前执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffRemoveAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffRemoveActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffRemoveActions;
            self.RunBuffActions(buffEntity, buffRemoveActionIds);
        }

        /// <summary>
        /// Buff刷新时执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffRefreshAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffRefreshActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffRefreshActions;
            self.RunBuffActions(buffEntity, buffRefreshActionIds);
        }

        /// <summary>
        /// BuffTick时执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffTickAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffTickActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTickActions;
            self.RunBuffActions(buffEntity, buffTickActionIds);
        }

        /// <summary>
        /// 获取Buff Tick Action列表
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        /// <param name="aBuffActionList"></param>
        /// <param name="argsList"></param>
        public static void GetBuffTickActions(this BuffActionDispatcher self, BuffEntity buffEntity, out List<ABuffAction> aBuffActionList, out List<int[]> argsList)
        {
            int[] buffTickActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTickActions;
            using var buffActionlist = ListComponent<ABuffAction>.Create();
            using var argsListComponent = ListComponent<int[]>.Create();
            for (int i = 0; i < buffTickActionIds.Length; i++)
            {
                BuffActionConfig buffActionConfig = BuffActionConfigCategory.Instance.Get(buffTickActionIds[i]);
                int baseBuffActionId = buffActionConfig.BaseActionId;
                int[] args = buffActionConfig.actionArgs;
                if (self.idBuffActions.TryGetValue(baseBuffActionId, out var baseBuffAction))
                {
                    buffActionlist.List.Add(baseBuffAction);
                    argsListComponent.List.Add(args);
                }
            }
            aBuffActionList = buffActionlist.List;
            argsList = argsListComponent.List;
        }

        /// <summary>
        /// Buff时间截至时执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffTimeOutAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffTimeOutActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTimeOutActions;
            self.RunBuffActions(buffEntity, buffTimeOutActionIds);
        }
    }
}