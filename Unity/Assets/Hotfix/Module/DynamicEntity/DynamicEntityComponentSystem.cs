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
            if (assetEntity!=null)
            {
                self.DynamicEntities.Add(assetEntity.Object.GetInstanceID(), dynamicEntity);
            }
        }
    }
}