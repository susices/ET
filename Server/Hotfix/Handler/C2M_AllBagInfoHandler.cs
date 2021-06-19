using System;

namespace ET
{
    /// <summary>
    /// 获取玩家背包所有物品
    /// </summary>
    public class C2M_AllBagInfoHandler : AMRpcHandler<C2M_AllBagInfo,M2C_AllBagInfo>
    {
        protected override async ETTask Run(Session session, C2M_AllBagInfo request, M2C_AllBagInfo response, Action reply)
        {
            using var list = ListComponent<BagItem>.Create();

            
            await ETTask.CompletedTask;
        }
    }
}