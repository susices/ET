using System;

namespace ET
{
    public class C2M_TestUnitNumericHandler : AMActorLocationRpcHandler<Unit, C2M_TestUnitNumeric,M2C_TestUnitNumeric>
    {
        protected override async ETTask Run(Unit unit, C2M_TestUnitNumeric request, M2C_TestUnitNumeric response, Action reply)
        {
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            int newLevel = numericComponent.GetAsInt(NumericType.Level) + 1;
            int newExp = numericComponent.GetAsInt(NumericType.Exp) + 100;
            int newGold = numericComponent.GetAsInt(NumericType.Gold) + 50;
            numericComponent.Set(NumericType.Level, newLevel);
            numericComponent.Set(NumericType.Exp,newExp);
            numericComponent.Set(NumericType.Gold,newGold);
            reply();
            await ETTask.CompletedTask;
        }
    }
}