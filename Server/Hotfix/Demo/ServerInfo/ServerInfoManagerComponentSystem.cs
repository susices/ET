namespace ET
{
    public class ServerInfoManagerComponentAwakeSystem : AwakeSystem<ServerInfoManagerComponent>
    {
        public override void Awake(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    public class ServerInfoManagerComponentDestroySystem : DestroySystem<ServerInfoManagerComponent>
    {
        public override void Destroy(ServerInfoManagerComponent self)
        {
            foreach (var serverInfo in self.ServerInfos)
            {
                serverInfo?.Dispose();
            }
            self.ServerInfos.Clear();
        }
    }

    public class ServerInfoManagerComponentLoadSystem : LoadSystem<ServerInfoManagerComponent>
    {
        public override void Load(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    public static class ServerInfoManagerComponentSystem
    {
        public static async ETTask Awake(this ServerInfoManagerComponent self)
        {
            var serverInfos = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<ServerInfo>(d => true);
            if (serverInfos==null || serverInfos.Count==0)
            {
                Log.Error("ServerInfo count is zero!");
                self.ServerInfos.Clear();
                var serverInfoConfigs = ConfigComponent.Instance.Tables.ServerInfoConfigCategory.DataList;
                foreach (var serverInfoConfig in serverInfoConfigs)
                {
                    ServerInfo serverInfo = self.AddChildWithId<ServerInfo>(serverInfoConfig.Id);
                    serverInfo.ServerName = serverInfoConfig.ServerName;
                    serverInfo.Status = (int)ServerStatus.Normal;
                    self.ServerInfos.Add(serverInfo);
                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(serverInfo);
                }
                return;
            }

            self.ServerInfos.Clear();
            foreach (var serverInfo in serverInfos)
            {
                self.AddChild(serverInfo);
                self.ServerInfos.Add(serverInfo);
            }
            await ETTask.CompletedTask;
        }
    }
}