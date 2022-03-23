using System;

namespace ET
{
    public class G2L_RemoveLoginRecordHandler : AMActorRpcHandler<Scene,G2L_RemoveLoginRecord,L2G_RemoveLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_RemoveLoginRecord request, L2G_RemoveLoginRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock, accountId.GetHashCode()))
            {
                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(request.AccountId);
                if (zone== request.ServerId)
                {
                    scene.GetComponent<LoginInfoRecordComponent>().Remove(request.AccountId);
                }
            }
            reply();
        }
    }
}