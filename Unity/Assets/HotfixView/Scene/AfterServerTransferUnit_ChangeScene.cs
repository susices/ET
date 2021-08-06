namespace ET
{
    public class AfterServerTransferUnit_ChangeScene:AEvent<EventType.AfterServerTransferUnit>
    {
        protected override async ETTask Run(EventType.AfterServerTransferUnit args)
        {
            await Game.Scene.GetComponent<SceneComponent>().ChangeScene(args.TransferMapIndex);
        }
    }
}