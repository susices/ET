using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ABInfoAwakeSystem: AwakeSystem<ABInfo, string, AssetBundle>
    {
        public override void Awake(ABInfo self, string uiPanelType, AssetBundle a)
        {
            self.AssetBundle = a;
            self.Name = uiPanelType;
            self.RefCount = 1;
            self.AlreadyLoadAssets = false;
        }
    }

    [ObjectSystem]
    public class ABInfoDestroySystem: DestroySystem<ABInfo>
    {
        public override void Destroy(ABInfo self)
        {
            //Log.Debug($"desdroy assetbundle: {this.Name}");

            self.RefCount = 0;
            self.Name = "";
            self.AlreadyLoadAssets = false;
            self.AssetBundle = null;
        }
    }
}