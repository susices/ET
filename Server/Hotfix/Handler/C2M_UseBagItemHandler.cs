using System;
using System.Collections.Generic;

namespace ET
{
    
    public class C2M_UseBagItemHandler : AMActorLocationRpcHandler<Unit, C2M_UseBagItem,M2C_UseBagItem>
    {
        protected override async ETTask Run(Unit unit, C2M_UseBagItem request, M2C_UseBagItem response, Action reply)
        {
            var result =  await unit.GetComponent<BagComponent>().UseBagItem(request.BagItemId, request.BagItemCount);
            if (result)
            {
                response.Error = ErrorCode.ERR_Success;
                response.BagItems = new List<BagItem>();
                response.BagItems.Add(new BagItem(){DataId = request.BagItemId, DataValue = -request.BagItemCount});
                reply();
            }
            else
            {
                response.Error = ErrorCode.ERR_UseBagItemError;
                reply();
            }
        }
    }
}