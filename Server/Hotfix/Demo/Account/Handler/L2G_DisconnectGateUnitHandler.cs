using System;

namespace ET
{
    
    public class L2G_DisconnectGateUnitHandler : AMActorRpcHandler<Scene,L2G_DisconnectGateUnit,G2L_DisconnectGateUnit>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnit request, G2L_DisconnectGateUnit response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, request.AccountId.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player player = playerComponent.Get(accountId);
                if (player==null)
                {
                    reply();
                    return;
                }
                scene.GetComponent<GateSessionKeyComponent>().Remove(accountId);
                Session gateSession = Game.EventSystem.Get(player.SessionInstanceId) as Session;

                if (gateSession!=null  && !gateSession.IsDisposed)
                {
                    gateSession.Send(new A2C_Disconnect()
                    {
                        Error = ErrorCode.ERR_OtherAccountLogin,
                    });
                    gateSession.GetComponent<SessionPlayerComponent>().IsNeedKickPlayer = true;
                    gateSession?.Disconnect().Coroutine();
                }
                
                player.SessionInstanceId = 0;
                player.AddComponent<PlayerOfflineOutTimeComponent>();
                reply();
            }
            await ETTask.CompletedTask;
        }
    }
}