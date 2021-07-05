using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UILogin)]
    public class UILoginEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            // await ResourcesComponent.Instance.LoadBundleAsync("assets/bundles/ui");
            // GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset("assets/bundles/ui", UIType.UILogin);
            // GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            var assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync("Assets/Bundles/UI/UILogin.prefab");

            UI ui = EntityFactory.CreateWithParent<UI, string, AssetEntity>(uiComponent, UIType.UILogin, assetEntity);
            ui.AddComponent<UILoginComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            //ResourcesComponent.Instance.UnloadBundle("assets/bundles/ui");
        }
    }
}