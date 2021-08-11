using System;
using System.Collections.Generic;

namespace ET
{
    public class C2D_TestHandler:AMActorRpcHandler<Scene, C2D_Test,D2C_Test>
    {
        protected override async ETTask Run(Scene scene, C2D_Test request, D2C_Test response, Action reply)
        {
            Log.Info($"{scene.Name} 收到消息: {request.TestMsg}");
            List<BagInfo> bagInfos = await Game.Scene.GetComponent<DBComponent>().Query<BagInfo>(scene.DomainZone(), d => d.Id != 0);
            Log.Info($"baginfos count:{bagInfos.Count.ToString()}");
            foreach (var bagInfo in bagInfos)
            {
                var bag =  await scene.GetComponent<DBCacheComponent>().Query<BagInfo>(bagInfo.Id);
                if (bag==null)
                {
                    continue;
                }
                Log.Info($"测试数据缓存查询: playerId:{bag.Id.ToString()}");
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}