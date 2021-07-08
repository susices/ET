﻿

namespace ET
{
	public class LoginFinish_CreateLobbyUI: AEvent<EventType.LoginFinish>
	{
		protected override async ETTask Run(EventType.LoginFinish args)
		{
			await UIHelper.ShowUIPanel(args.ZoneScene, UiType.UILobby);
		}
	}
}
