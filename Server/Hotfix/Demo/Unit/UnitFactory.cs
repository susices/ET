using System;
using UnityEngine;

namespace ET
{
    public static class UnitFactory
    {
        public static Unit Create(Scene scene, long id, UnitType unitType)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            switch (unitType)
            {
                case UnitType.Player:
                {
                    Unit unit = unitComponent.AddChildWithId<Unit, int>(id, 1001);
                    // unit.AddComponent<MoveComponent>();
                    // unit.Position = new Vector3(-10, 0, -10);
			
                    NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
                    foreach (var playerNumericConfig in ConfigComponent.Instance.Tables.PlayerNumericConfigCatrgory.DataList)
                    {
                        if (playerNumericConfig.BaseValue==0)
                        {
                            continue;
                        }

                        if (playerNumericConfig.Id<3000)
                        {
                            int baseKey = playerNumericConfig.Id * 10 + 1;
                            numericComponent.SetNoEvent(baseKey, playerNumericConfig.BaseValue);
                        }
                        else
                        {
                            numericComponent.SetNoEvent(playerNumericConfig.Id, playerNumericConfig.BaseValue);
                        }
                    }
                    
                    var unitConfig = ConfigComponent.Instance.Tables.UnitConfigCategory.Get(1001);
                    unitComponent.Add(unit);
                    // 加入aoi
                    // unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);
                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }
    }
}