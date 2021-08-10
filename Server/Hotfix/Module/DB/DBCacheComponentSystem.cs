using System.Collections.Generic;

namespace ET
{
    public class DBCacheComponentAwakeSystem:AwakeSystem<DBCacheComponent>
    {
        public override void Awake(DBCacheComponent self)
        {
            self.LRUCapacity = FrameworkConfigVar.LRUCapacity.IntVar();
        }
    }

    public static class DBCacheComponentSystem
    {
        public static async ETTask<T> Query<T>(this DBCacheComponent self, long playerId) where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
                
            }
            return null;
        }

        public static async ETTask Save<T>(this DBCacheComponent self, long playerId,T Entity) where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
                
            }
        }
        
        public static async ETTask Save(this DBCacheComponent self, long playerId,List<Entity> entities)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
                
            }
        }

        public static async ETTask ClearCache(this DBCacheComponent self, long playerId)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBCache, playerId))
            {
                
            }
        }

    }
}