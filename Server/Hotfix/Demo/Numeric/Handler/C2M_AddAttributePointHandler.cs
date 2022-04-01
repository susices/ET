using System;

namespace ET
{
    public class C2M_AddAttributePointHandler : AMActorLocationRpcHandler<Unit,C2M_AddAttributePoint,M2C_AddAttributePoint>
    {
        protected override async ETTask Run(Unit unit, C2M_AddAttributePoint request, M2C_AddAttributePoint response, Action reply)
        {
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            int targetNumericType = request.NumericType;
            var config = ConfigComponent.Instance.Tables.PlayerNumericConfigCatrgory.GetOrDefault(targetNumericType);
            if (config==null)
            {
                response.Error = ErrorCode.ERR_NumericTypeNotExist;
                reply();
                return;
            }

            if (!config.IsAddPoint)
            {
                response.Error = ErrorCode.ERR_NumericTypeNotAddPoint;
                reply();
                return;
            }

            int attributePoint = numericComponent.GetAsInt(NumericType.AttributePoint);
            if (attributePoint<=0)
            {
                response.Error = ErrorCode.ERR_AddPointNotEnough;
                reply();
                return;
            }

            attributePoint--;
            numericComponent.Set(NumericType.AttributePoint, attributePoint);
            int targetNumericValue = numericComponent.GetAsInt(targetNumericType) + 1;
            numericComponent.Set(targetNumericType, targetNumericValue);
            //await numericComponent.AddOrUpdateUnitCache();
            await ETTask.CompletedTask;
            reply();
        }
    }
}