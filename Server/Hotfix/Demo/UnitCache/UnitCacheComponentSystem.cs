namespace ET
{
    public class UnitCacheComponentAwakeSystem : AwakeSystem<UnitCacheComponent>
    {
        public override void Awake(UnitCacheComponent self)
        {
            self.UnitKeyList.Clear();
            foreach (var type in Game.EventSystem.GetTypes().Values)
            {
                if (type!=typeof(IUnitCache) && typeof(IUnitCache).IsAssignableFrom(type))
                {
                    self.UnitKeyList.Add(type.Name);
                }
            }

            foreach (string key in self.UnitKeyList)
            {
                UnitCache unitCache = self.AddChild<UnitCache>();
                unitCache.Key = key;
                self.UnitCaches.Add(key,unitCache);
            }
        }
    }

    public class UnitCacheComponentDestroySystem : DestroySystem<UnitCacheComponent>
    {
        public override void Destroy(UnitCacheComponent self)
        {
            foreach (var unitCache in self.UnitCaches.Values)
            {
                unitCache?.Dispose();
            }
            self.UnitCaches.Clear();
        }
    }

    public static class UnitCacheComponentSystem
    {
        public static async ETTask AddOrUpdateUnitCache(this UnitCacheComponent self,long unitId, ListComponent<Entity> entities)
        {
            using (var list = ListComponent<Entity>.Create())
            {
                if (entities.Count==0)
                {
                    return;
                }
                
                foreach (var entity in entities)
                {
                    string key = entity.GetType().Name;
                    if (!self.UnitCaches.TryGetValue(key, out UnitCache unitCache ))
                    {
                        unitCache = self.AddChild<UnitCache>();
                        unitCache.Key = key;
                        self.UnitCaches.Add(key,unitCache);
                    }
                    unitCache.AddOrUpdate(entity);
                    list.Add(entity);
                }
                await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(unitId, list);
            }
            await ETTask.CompletedTask;
        }

        public static async ETTask<Entity> Get(this UnitCacheComponent self, long unitId, string componentName)
        {
            if (!self.UnitCaches.TryGetValue(componentName, out UnitCache unitCache))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.Key = componentName;
                self.UnitCaches.Add(componentName,unitCache);
            }

            return await unitCache.Get(unitId);
        }

        public static void Delete(this UnitCacheComponent self, long unitId)
        {
            foreach (var unitCache in self.UnitCaches.Values)
            {
                unitCache.Delete(unitId);
            }
        }
    }
}