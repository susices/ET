using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class DynamicEntityComponentAwakeSystem : AwakeSystem<DynamicEntityComponent>
    {
        public override void Awake(DynamicEntityComponent self)
        {
            self.DynamicEntities = new Dictionary<int, Entity>();
        }
    }

    public static class DynamicEntityComponentSystem
    {
        public static void AddEntiyToDic(this DynamicEntityComponent self, Entity dynamicEntity)
        {
            var assetEntity = dynamicEntity.GetComponent<AssetEntity>();

            if (assetEntity==null)
            {
                Log.Error($"动态实体 缺少 AssetEntity组件 entityId:{dynamicEntity.Id.ToString()}");
                return;
            }

            if (!self.DynamicEntities.ContainsKey(assetEntity.Object.GetInstanceID()))
            {
                self.DynamicEntities.Add(assetEntity.Object.GetInstanceID(), dynamicEntity);
            }
            else
            {
                Log.Error($"重复添加GameObject EntityId:{dynamicEntity.Id.ToString()} GobjInstanceId:{assetEntity.Object.GetInstanceID().ToString()} ");
                return;
            }
        }
    }
}