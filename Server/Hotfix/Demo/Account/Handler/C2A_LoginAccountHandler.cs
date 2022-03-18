using System;
using System.Text.RegularExpressions;

namespace ET
{
    [MessageHandler]
    public class C2A_LoginAccountHandler: AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误, 当前SceneType{session.DomainScene().SceneType}");
                session.Dispose();
            }

            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoIsNullError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            if (!Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_AccountNameFormError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))
            {
                response.Error = ErrorCode.ERR_PasswordFormError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                {
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<Account>(x => x.AccountName.Equals(request.AccountName.Trim()));

                    Account account = null;
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);
                        if (account.AccountType == (int)AccountType.Blacklist)
                        {
                            response.Error = ErrorCode.ERR_AccountInBlacklistError;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName;
                        account.Password = request.Password;
                        account.CreateTime = TimeHelper.ServerNow();
                        account.AccountType = (int)AccountType.General;
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(account);
                    }
                    
                    var sceneConfig =  ConfigComponent.Instance.Tables.StartSceneConfigCategory.GetBySceneName(session.DomainZone(), "LoginCenter");
                    long loginCenterInstanceId = sceneConfig.InstanceId;
                    var l2a_loginAccountResponse = (L2A_LoginAccountResponse) await ActorMessageSenderComponent.Instance.Call(loginCenterInstanceId, new A2L_LoginAccountRequest() { AccountId = account.Id });
                    if (l2a_loginAccountResponse.Error!=ErrorCode.ERR_Success)
                    {
                        response.Error = l2a_loginAccountResponse.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        account?.Dispose(); 
                        return;
                    }
                    
                    long accountSessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(account.Id);
                    Session otherSession = EventSystem.Instance.Get(accountSessionInstanceId) as Session;
                    otherSession?.Send(new A2C_Disconnect(){Error = 0});
                    otherSession?.Disconnect().Coroutine();
                    
                    session.DomainScene().GetComponent<AccountSessionsComponent>().Add(account.Id, session.InstanceId);
                    session.AddComponent<AccountCheckOutTimeComponent, long>(account.Id);
                    
                    string Token = $"{TimeHelper.ServerNow().ToString()}{RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString()}";
                    session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
                    session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, Token);
                    response.AccountId = account.Id;
                    response.Token = Token;
                    reply();
                    account?.Dispose();
                }
            }
        }
    }
}