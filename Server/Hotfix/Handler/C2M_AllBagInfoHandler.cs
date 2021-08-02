using System;

namespace ET
{
    /// <summary>
    /// 获取玩家背包所有物品
    /// </summary>
    public class C2M_AllBagInfoHandler: AMActorLocationRpcHandler<Unit,C2M_AllBagInfo, M2C_AllBagInfo>
    {
        protected override async ETTask Run(Unit unit, C2M_AllBagInfo request, M2C_AllBagInfo response, Action reply)
        {
            var bagInfo = unit.GetComponent<BagComponent>().BagInfo;
            if (bagInfo==null)
            {
                response.Error = ErrorCode.ERR_BagInfoError;
                reply();
                return;
            }

            response.BagItems = bagInfo.BagItems;
            response.Error = ErrorCode.ERR_Success;
            reply();
            await ETTask.CompletedTask;
        }
    }
}