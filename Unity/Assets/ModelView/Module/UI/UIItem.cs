namespace ET
{
    /// <summary>
    /// UI物体
    /// </summary>
    public class UIItem : Entity
    {
        public AssetEntity UIItemAssetEntity;
        
        public int UIItemType;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            this.UIItemAssetEntity.Dispose();
            this.UIItemAssetEntity = null;
            this.UIItemType = 0;
        }
    }
}