using System;

namespace ET
{
    [MessageHandler]
    public class C2G_LoginGameGateHandler : AMRpcHandler<C2G_LoginGameGate,G2C_LoginGameGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGameGate request, G2C_LoginGameGate response, Action reply)
        {
            if (session.DomainScene().SceneType!= SceneType.Gate)
            {
                Log.Error($"请求的Scene错误, 当前SceneType{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            if (session.GetComponent<SessionLockingComponent>()!=null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                return;
            }

            string tokenKey = session.DomainScene().GetComponent<GateSessionKeyComponent>().Get(request.Account);
            if (tokenKey==null || !tokenKey.Equals(request.Key))
            {
                response.Error = ErrorCode.ERR_ConnectGateKeyError;
                response.Message = "Gate key 验证失败";
                reply();
                session?.Disconnect().Coroutine();
                return;
            }
            
            session.DomainScene().GetComponent<GateSessionKeyComponent>().Remove(request.Account);
            long instanceId = session.InstanceId;
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, request.Account.GetHashCode()))
                {
                    if (instanceId!=session.InstanceId)
                    {
                        return;
                    }

                    StartSceneConfig loginCenterConfig = ConfigComponent.Instance.Tables.StartSceneConfigCategory.LoginCenterConfig;
                    L2G_AddLoginRecord l2GAddLoginRecord = (L2G_AddLoginRecord)await MessageHelper.CallActor(loginCenterConfig.InstanceId,
                        new G2L_AddLoginRecord() { AccountId = request.Account,ServerId = session.DomainZone()});

                    Log.Debug("After l2GAddLoginRecord");
                    if (l2GAddLoginRecord.Error!=ErrorCode.ERR_Success)
                    {
                        response.Error = l2GAddLoginRecord.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        Log.Debug("l2GAddLoginRecord.Error!=ErrorCode.ERR_Success");
                        return;
                    }

                    SessionStateComponent sessionStateComponent = session.GetComponent<SessionStateComponent>();
                    if (sessionStateComponent==null)
                    {
                        sessionStateComponent = session.AddComponent<SessionStateComponent>();
                    }

                    sessionStateComponent.State = SessionState.Normal;
                    
                    
                    Player player = session.DomainScene().GetComponent<PlayerComponent>().Get(request.Account);
                    
                    if (player==null)
                    {
                        Log.Debug("player==null");
                        player = session.DomainScene().GetComponent<PlayerComponent>()
                                .AddChildWithId<Player, long, long>(request.RoleId, request.Account, request.RoleId);
                        player.PlayerState = PlayerState.Gate;
                        session.DomainScene().GetComponent<PlayerComponent>().Add(player);
                        session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
                    }
                    else
                    {
                        Log.Debug("player!=null");
                        session.RemoveComponent<PlayerOfflineOutTimeComponent>();
                    }

                    var sessionPLayerComponent = session.AddComponent<SessionPlayerComponent>();
                    sessionPLayerComponent.PlayerId = player.Id;
                    sessionPLayerComponent.PlayerInstanceId = player.InstanceId;
                    sessionPLayerComponent.AccountId = request.Account;
                    player.SessionInstanceId = session.InstanceId;
                    reply();
                }
            }
            await ETTask.CompletedTask;
        }
    }
}