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
            var baginfos = await Game.Scene.GetComponent<DBComponent>().Query<BagInfo>(d => d.PlayerId == session.GetComponent<SessionPlayerComponent>().Player.Id);
            if (baginfos.Count!=1)
            {
                response.Error = ErrorCode.ERR_BagInfoError;
                reply();
                return;
            }
            response.BagItems = baginfos[0].BagItems;
            response.Error = ErrorCode.ERR_Success;
            reply();
            await ETTask.CompletedTask;
        }
    }
}