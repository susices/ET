using System;

namespace ET
{
    [MessageHandler]
    public class C2G_EnterGameHandler : AMRpcHandler<C2G_EnterGame,G2C_EnterGame>
    {
        protected override async ETTask Run(Session session, C2G_EnterGame request, G2C_EnterGame response, Action reply)
        {
            if (session.DomainScene().SceneType!= SceneType.Gate)
            {
                Log.Error($"请求的Scene错误, Scene为: {session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            if (session.GetComponent<SessionLockingComponent>()!=null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                return;
            }

            SessionPlayerComponent sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
            if (sessionPlayerComponent==null)
            {
                response.Error = ErrorCode.ERR_SessionPlayerComponentError;
                reply();
                return;   
            }

            Player player = Game.EventSystem.Get(sessionPlayerComponent.PlayerInstanceId) as Player;
            if (player==null || player.IsDisposed)
            {
                response.Error = ErrorCode.ERR_NonePlayerError;
                reply();
                return;
            }

            long instanceId = session.InstanceId;

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.Account.GetHashCode()))
                {
                    if (instanceId!=session.InstanceId || player.IsDisposed)
                    {
                        response.Error = ErrorCode.ERR_PlayerSessionDisposeError;
                        return ;
                    }

                    if (session.GetComponent<SessionStateComponent>()!=null && session.GetComponent<SessionStateComponent>().State == SessionState.Game)
                    {
                        response.Error = ErrorCode.ERR_SessionStateError;
                        reply();
                        return;
                    }

                    if (player.PlayerState == PlayerState.Game)
                    {
                        try
                        {
                            IActorResponse reqEnter = await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestEnterGameState());
                            if (reqEnter.Error==ErrorCode.ERR_Success)
                            {
                                reply();
                                return;
                            }
                            
                            Log.Error($"二次登录失败  {reqEnter.Error} | {reqEnter.Message}");
                            response.Error = ErrorCode.ERR_ReEnterGameError;
                            await DisconnectHelper.KickPlayer(player, true);
                            reply();
                            session?.Disconnect().Coroutine();
                        }
                        catch (Exception e)
                        {
                            Log.Error($"二次登录失败 {e.ToString()}");
                            response.Error = ErrorCode.ERR_ReEnterGameError2;
                            await DisconnectHelper.KickPlayer(player, true);
                            reply();
                            session?.Disconnect().Coroutine();
                            throw;
                        }
                        return;
                    }

                    try
                    {
                        (bool isNewPlayer, Unit unit) = await UnitHelper.LoadUnit(player);
                        unit.AddComponent<UnitGateComponent, long>(player.InstanceId);

                        await UnitHelper.InitUnit(unit, isNewPlayer);
                        response.MyId = unit.Id;
                        reply();
                        
                        StartSceneConfig startSceneConfig =
                                ConfigComponent.Instance.Tables.StartSceneConfigCategory.GetBySceneName(session.DomainZone(), "Game");
                        await TransferHelper.Transfer(unit, startSceneConfig.InstanceId, startSceneConfig.Name);
                        player.UnitId = unit.Id;
                        
                        SessionStateComponent sessionStateComponent = session.GetComponent<SessionStateComponent>();
                        if (sessionStateComponent==null)
                        {
                            sessionStateComponent = session.AddComponent<SessionStateComponent>();
                        }
                        sessionStateComponent.State = SessionState.Game;
                        player.PlayerState = PlayerState.Game;

                    }
                    catch (Exception e)
                    {
                        Log.Error($"角色进入逻辑服出现问题 账号ID: {player.Account} 角色ID:{player.Id} 异常信息：{e.ToString()}");
                        response.Error = ErrorCode.ERR_EnterGameError;
                        reply();
                        await DisconnectHelper.KickPlayer(player, true);
                        session?.Disconnect().Coroutine();
                    }
                }
            }
            await ETTask.CompletedTask;
        }
    }
}