using UnityEngine;

namespace ET
{
	public class EnterMapFinish_RemoveLobbyUI: AEvent<EventType.EnterMapFinish>
	{
		protected override async ETTask Run(EventType.EnterMapFinish args)
		{
			// 加载初始场景
			await Game.Scene.GetComponent<SceneComponent>().ChangeScene(1);
			args.ZoneScene.GetComponent<TransferComponent>().CurrentMapIndex = 1;
			args.ZoneScene.AddComponent<OperaComponent>();
			await args.ZoneScene.RemoveUIPanel(UIPanelType.UILobby);
		}
	}
}
