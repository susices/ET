

namespace ET
{
	public class LoginFinish_CreateLobbyUI: AEvent<EventType.LoginFinish>
	{
		protected override async ETTask Run(EventType.LoginFinish args)
		{
			await args.ZoneScene.ShowUIPanel(UIPanelType.UILobby);
		}
	}
}
