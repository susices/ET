using System;

namespace ET.Handler
{
    public class C2A_GetServerInfosHandler : AMRpcHandler<C2A_GetServerInfos, A2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfos request, A2C_GetServerInfos response, Action reply)
        {
            if (session.DomainScene().SceneType!= SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);
            if (token==null || token!=request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }

            foreach (var serverInfo in session.DomainScene().GetComponent<ServerInfoManagerComponent>().ServerInfos)
            {
                response.ServerInfoProtoList.Add(serverInfo.ToMessage());
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}