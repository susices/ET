using System;

namespace ET
{
    
    [MessageHandler]
    public class C2R_RegisterHandler: AMRpcHandler<C2R_Register, R2C_Register>
    {
        protected override async ETTask Run(Session session, C2R_Register request, R2C_Register response, Action reply)
        {
            var account = request.Account;
            var password = request.Password;
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                response.Error = ErrorCode.ERR_AccountPassWordError;
                reply();
                return;
            }

            var accountList = await Game.Scene.GetComponent<DBComponent>().Query<AccountInfo>(d =>d.AccountName == account);
            if (accountList.Count > 0)
            {
                response.Error = ErrorCode.ERR_AccountPassWordError;
                reply();
                return;
            }

            try
            {
                AccountInfo accountInfo = EntityFactory.CreateWithId<AccountInfo>(session.Domain, IdGenerater.Instance.GenerateId());
                
                accountInfo.AccountName = account;
                accountInfo.Password = password;
                await Game.Scene.GetComponent<DBComponent>().Save(accountInfo);

            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
                response.Error = ErrorCode.ERR_AccountPassWordError;
                response.Message = e.ToString();
                reply();
                return;
            }
            
            response.Error = ErrorCode.ERR_Success;
            reply();
        }
    }
}