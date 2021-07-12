namespace ET
{
    public class UIItemAwakeSystem : AwakeSystem<UIItem, int, AssetEntity>
    {
        public override void Awake(UIItem self, int uiItemType, AssetEntity UIAssetEntity)
        {
            self.UIItemAssetEntity = UIAssetEntity;
            self.UIItemType = uiItemType;
        }
    }

    public static class UIItemSystem
    {
        
    }
}