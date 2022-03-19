namespace ET
{
    public class ServerInfoComponentDestroySystem : DestroySystem<ServerInfosComponent>
    {
        public override void Destroy(ServerInfosComponent self)
        {
            foreach (var serverInfo in self.ServerInfoList)
            {
                serverInfo?.Dispose();
            }
            
            self.ServerInfoList.Clear();
        }
    }

    public static class ServerInfoComponentSystem
    {
        public static void Add(this ServerInfosComponent self, ServerInfo serverInfo)
        {
            self.ServerInfoList.Add(serverInfo);
        }
    }
}