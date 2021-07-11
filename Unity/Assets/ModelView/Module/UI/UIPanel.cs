namespace ET
{
    public sealed class UIPanel: Entity
    {
        public AssetEntity UIPanelAssetEntity;

        public int UIPanelType;

        public bool IsSubPanel;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.UIPanelAssetEntity.Dispose();
            this.UIPanelAssetEntity = null;
            this.UIPanelType = 0;
            this.IsSubPanel = false;
        }
    }
}