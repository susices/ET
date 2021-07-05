using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UILobby)]
    public class UILobbyEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            // await ETTask.CompletedTask;
            // ResourcesComponent.Instance.LoadBundle("assets/bundles/ui");
            // GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset("assets", UIType.UILobby);
            //
            // GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            var assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync("Assets/Bundles/UI/UILobby.prefab");
            UI ui = EntityFactory.CreateWithParent<UI, string, AssetEntity>(uiComponent, UIType.UILobby, assetEntity);

            ui.AddComponent<UILobbyComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle("assets/bundles/ui");
        }
    }
}