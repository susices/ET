using System;

namespace ET
{
    public static class UnitCacheHelper
    {
        public static async ETTask AddOrUpdateUnitCache<T>(this T self) where T:Entity,IUnitCache
        {
            UnitCache2Other_AddOrUpdateUnit unitCache2OtherAddOrUpdateUnit = null;
            try
            {
                Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit()
                {
                    UnitId = self.Id,
                };
                message.EntityTypes.Add(typeof(T).FullName);
                message.EntityBytes.Add(MongoHelper.ToBson(self));
                unitCache2OtherAddOrUpdateUnit =(UnitCache2Other_AddOrUpdateUnit) await MessageHelper.CallActor(ConfigComponent.Instance.Tables.StartSceneConfigCategory.GetUnitCacheConfig(self.Id).InstanceId, message);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return;
            }

            if (unitCache2OtherAddOrUpdateUnit.Error!=ErrorCode.ERR_Success)
            {
                Log.Error(unitCache2OtherAddOrUpdateUnit.Error.ToString());
                return;
            }
        }

        public static async ETTask<T> GetUnitComponentCache<T>(long unitId) where T : Entity, IUnitCache
        {
            UnitCache2Other_GetUnit unitCache2OtherGetUnit = null;
            try
            {
                Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit() { UnitId = unitId };
                message.ComponentNameList.Add(typeof(T).FullName);
                unitCache2OtherGetUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(
                    ConfigComponent.Instance.Tables.StartSceneConfigCategory.GetUnitCacheConfig(unitId).InstanceId,
                    message);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return null;
            }
            
            if (unitCache2OtherGetUnit.Error!=ErrorCode.ERR_Success)
            {
                Log.Error(unitCache2OtherGetUnit.Error.ToString());
                return null;
            }

            if (unitCache2OtherGetUnit.EntityList.Count<1)
            {
                return null;
            }

            return unitCache2OtherGetUnit.EntityList[0] as T;
        }


        public static async ETTask DeleteUnitCache(long unitId)
        {
            UnitCache2Other_DeleteUnit unitCache2OtherDeleteUnit = null;
            try
            {
                Other2UnitCache_DeleteUnit message = new Other2UnitCache_DeleteUnit() { UnitId = unitId };
                unitCache2OtherDeleteUnit = (UnitCache2Other_DeleteUnit)await MessageHelper.CallActor(
                    ConfigComponent.Instance.Tables.StartSceneConfigCategory.GetUnitCacheConfig(unitId).InstanceId,
                    message);
            } 
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return;
            }

            if (unitCache2OtherDeleteUnit.Error!=ErrorCode.ERR_Success)
            {
                Log.Error(unitCache2OtherDeleteUnit.Error.ToString());
                return;
            }
        }

        public static async ETTask<Unit> GetUnitCache(Scene scene, long unitId)
        {
            long unitCacheInstanceID = ConfigComponent.Instance.Tables.StartSceneConfigCategory.GetUnitCacheConfig(unitId).InstanceId;
            Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit() { UnitId = unitId };
            UnitCache2Other_GetUnit unitCache2OtherGetUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(unitCacheInstanceID, message);
            if (unitCache2OtherGetUnit.Error!=ErrorCode.ERR_Success)
            {
                Log.Error(unitCache2OtherGetUnit.Error.ToString());
                return null;
            }

            if (unitCache2OtherGetUnit.EntityList.Count==0)
            {
                return null;
            }
            
            Unit unit = unitCache2OtherGetUnit.EntityList[0] as Unit;
            if (unit==null)
            {
                return null;
            }

            scene.AddChild(unit);

            foreach (var entity in unitCache2OtherGetUnit.EntityList)
            {
                if (entity==null ||entity is Unit)
                {
                    continue;
                }
                unit.AddComponent(entity);
            }
            return unit;
        }

        public static void AddOrUpdateUnitAllCache(Unit unit)
        {
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit() { UnitId = unit.Id };
            message.EntityTypes.Add(unit.GetType().FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(unit));
            foreach ((Type type, Entity entity) in unit.Components)
            {
                if (!typeof(IUnitCache).IsAssignableFrom(type))
                {
                    continue;
                }
                message.EntityTypes.Add(type.FullName);
                message.EntityBytes.Add(MongoHelper.ToBson(entity));
            }
            MessageHelper.CallActor(ConfigComponent.Instance.Tables.StartSceneConfigCategory.GetUnitCacheConfig(unit.Id).InstanceId, message).Coroutine();
        }
    }
}