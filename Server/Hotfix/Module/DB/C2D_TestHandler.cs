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

            //await RunTestCache(scene, bagInfos);
            long time = TimeHelper.ServerNow();
            using var list = ListComponent<ETTask>.Create();
            for (int i = 0; i < 1000; i++)
            {
                list.List.Add(RunTestCache(scene,bagInfos));
            }
            await ETTaskHelper.WaitAll(list.List);
            Log.Info($"总计用时：{(TimeHelper.ServerNow()-time).ToString()}");
            reply();
            await ETTask.CompletedTask;
        }

        public static async ETTask RunTestCache(Scene scene, List<BagInfo> bagInfos)
        {
            int index = RandomHelper.RandomNumber(0, 50);
            var bag =  await scene.GetComponent<DBCacheComponent>().Query<BagInfo>(bagInfos[index].Id);
        }
        
        public static async ETTask RunTestDB(Scene scene, List<BagInfo> bagInfos)
        {
            int index = RandomHelper.RandomNumber(0, bagInfos.Count);
            var bag =  await Game.Scene.GetComponent<DBComponent>().Query<BagInfo>(scene.DomainZone(),bagInfos[index].Id);
        }
    }
}