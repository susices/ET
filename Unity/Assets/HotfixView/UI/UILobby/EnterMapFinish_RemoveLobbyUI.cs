using UnityEngine;

namespace ET
{
	public class EnterMapFinish_RemoveLobbyUI: AEvent<EventType.EnterMapFinish>
	{
		protected override async ETTask Run(EventType.EnterMapFinish args)
		{
			//wenchao 修改加载场景
			// 加载场景资源
			await ResourcesComponent.Instance.LoadBundleAsync(SceneConfigCategory.Instance.Get(1).AssetPath.LocalizedAssetPath());
			// 切换到map场景
			using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
			{
				await sceneChangeComponent.ChangeSceneAsync("Map");
			}
            args.ZoneScene.AddComponent<OperaComponent>();
            var session = args.ZoneScene.GetComponent<SessionComponent>().Session;
            var m2cAllBagInfo = (M2C_AllBagInfo)await session.Call(new C2M_AllBagInfo());
            await args.ZoneScene.RemoveUIPanel(UiPanelComponentIndex.UILobby);
		}
	}
}
