using System;

namespace ET
{
    public class G2L_AddLoginRecordHandler : AMActorRpcHandler<Scene,G2L_AddLoginRecord,L2G_AddLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_AddLoginRecord request, L2G_AddLoginRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock, request.AccountId.GetHashCode()))
            {
                scene.GetComponent<LoginInfoRecordComponent>().Remove(request.AccountId);
                scene.GetComponent<LoginInfoRecordComponent>().Add(request.AccountId, request.ServerId);    
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}