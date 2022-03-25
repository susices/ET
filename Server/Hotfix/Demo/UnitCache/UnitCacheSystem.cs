using System.Collections.Generic;

namespace ET
{
    public class UnitCacheDestroySystem: DestroySystem<UnitCache>
    {
        public override void Destroy(UnitCache self)
        {
            self.Key = null;
            foreach (var entity in self.CacheComponentDic.Values)
            {
                entity.Dispose();
            }
            self.CacheComponentDic.Clear(); 
        }
    }

    public static class UnitCacheSystem
    {
        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if (entity==null  || entity.IsDisposed)
            {
                return;
            }

            if (self.CacheComponentDic.TryGetValue(entity.Id, out Entity oldEntity))
            {
                if (oldEntity!=entity)
                {
                    oldEntity.Dispose();
                }
                self.CacheComponentDic.Remove(oldEntity.Id);
            }
            self.CacheComponentDic.Add(entity.Id,entity);
        }

        public static async ETTask<Entity> Get(this UnitCache self, long unitId)
        {
            Entity entity = null;
            if (!self.CacheComponentDic.TryGetValue(unitId, out entity))
            {
                entity = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<Entity>(unitId, self.Key);
                if (entity!=null)
                {
                    self.AddOrUpdate(entity);
                }
            }
            return entity;
        }

        public static void Delete(this UnitCache self, long unitId)
        {
            if (self.CacheComponentDic.TryGetValue(unitId, out Entity entity))
            {
                entity.Dispose();
                self.CacheComponentDic.Remove(unitId);
            }
        }
    }
}