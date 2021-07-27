using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETVoid Login(Scene zoneScene, string address, string account, string password)
        {
            try
            {
                // 创建一个ETModel层的Session
                R2C_Login r2CLogin;
                using (Session session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address)))
                {
                    r2CLogin = (R2C_Login) await session.Call(new C2R_Login() { Account = account, Password = password });
                }

                // 创建一个gate Session,并且保存到SessionComponent中
                Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2CLogin.Address));
                
                zoneScene.AddComponent<SessionComponent>().Session = gateSession;
				
                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(
                    new C2G_LoginGate() { Key = r2CLogin.Key, GateId = r2CLogin.GateId, PlayerId = r2CLogin.PlayerId});
                gateSession.AddComponent<PingComponent>();
                Log.Info("登陆gate成功!");
                Log.Info(g2CLoginGate.PlayerId.ToString());
                await Game.EventSystem.Publish(new EventType.LoginFinish() {ZoneScene = zoneScene});
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        
        public static async ETVoid Register(Scene zoneScene, string address,string account, string password)
        {
            // 创建一个ETModel层的Session
            R2C_Register r2CRegister;
            using (Session session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address)))
            {
                r2CRegister = (R2C_Register) await session.Call(new C2R_Register() { Account = account, Password = password});
            }
            if (r2CRegister.Error == ErrorCode.ERR_Success)
            {
                Log.Debug("注册成功！");
            }
            else
            {
                Log.Error("注册失败！");
            }
        }
    }
}