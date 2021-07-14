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
    
    public class UIItemDestroySystem: DestroySystem<UIItem>
    {
        public override void Destroy(UIItem self)
        {
            self.UIItemAssetEntity.Dispose();
            self.UIItemAssetEntity = null;
            self.UIItemType = 0;
        }
    }

    public static class UIItemSystem
    {
        
    }
}