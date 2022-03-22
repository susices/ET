using System;

namespace ET
{
    [MessageHandler]
    public class C2R_LoginRealmHandler: AMRpcHandler<C2R_LoginRealm,R2C_LoginRealm>
    {
        protected override async ETTask Run(Session session, C2R_LoginRealm request, R2C_LoginRealm response, Action reply)
        {
            if (session.DomainScene().SceneType!= SceneType.Realm)
            {
                Log.Error($"请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            Scene domainScene = session.DomainScene();
            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);
            if (token==null || token!=request.RealmTokenKey)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }
            
            domainScene.GetComponent<TokenComponent>().Remove(request.AccountId);
            
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginRealm, request.AccountId.GetHashCode()))
                {
                    StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(session.DomainZone(), request.AccountId);

                    var g2RGetLoginGateKey = (G2R_GetLoginGateKey) await MessageHelper.CallActor(gateConfig.InstanceId, new R2G_GetLoginGateKey()
                    {
                        AccountId = request.AccountId,
                    });

                    if (g2RGetLoginGateKey.Error!=ErrorCode.ERR_Success)
                    {
                        response.Error = g2RGetLoginGateKey.Error;
                        reply();
                        return;
                    }

                    response.GateSessionKey = g2RGetLoginGateKey.GateSessionKey;
                    response.GateAddress = gateConfig.OuterIPPort.ToString();
                    reply();
                    session?.Disconnect().Coroutine();
                }
            }
        }
    }
}