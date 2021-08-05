using UnityEngine;

namespace ET
{
	public class EnterMapFinish_RemoveLobbyUI: AEvent<EventType.EnterMapFinish>
	{
		protected override async ETTask Run(EventType.EnterMapFinish args)
		{
			//wenchao 修改加载场景
			// 加载场景资源
			var sceneConfig = UnitySceneConfigCategory.Instance.Get(1);
			await ResourcesComponent.Instance.LoadBundleAsync(sceneConfig.AssetPath.LocalizedAssetPath());
			// 切换到map场景
			using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
			{
				await sceneChangeComponent.ChangeSceneAsync(sceneConfig.SceneName);
			}
            args.ZoneScene.AddComponent<OperaComponent>();
            var session = args.ZoneScene.GetComponent<SessionComponent>().Session;
            await args.ZoneScene.RemoveUIPanel(UIPanelType.UILobby);
		}
	}
}
