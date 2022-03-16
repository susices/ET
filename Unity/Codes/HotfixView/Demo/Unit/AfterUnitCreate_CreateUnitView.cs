using BM;
using UnityEngine;

namespace ET
{
    public class AfterUnitCreate_CreateUnitView: AEvent<EventType.AfterUnitCreate>
    {
        protected override async ETTask Run(EventType.AfterUnitCreate args)
        {
            // Unit View层
            switch (args.Unit.Type)
            {
                case UnitType.Player:
                {
                    GameObject bundleGameObject = await AssetComponent.LoadAsync<GameObject>("Assets/Bundles/Unit/Unit.prefab");
                    GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");
                    GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
                    go.transform.position = args.Unit.Position;
                    args.Unit.AddComponent<GameObjectComponent>().GameObject = go;
                    args.Unit.AddComponent<AnimatorComponent>();
                }
                    break;
                case UnitType.NPC:
                {
                    GameObject bundleGameObject = await AssetComponent.LoadAsync<GameObject>("Assets/Bundles/Unit/Unit.prefab");
                    GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");
                    GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
                    go.transform.position = args.Unit.Position;
                    args.Unit.AddComponent<GameObjectComponent>().GameObject = go;
                    args.Unit.AddComponent<AnimatorComponent>();
                }
                    break;

            }
            
            
            await ETTask.CompletedTask;
        }
    }
}