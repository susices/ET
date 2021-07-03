using UnityEngine;

namespace ET
{
    public class ABInfo: Entity
    {
        public string Name { get; set; }

        public int RefCount { get; set; }

        public AssetBundle AssetBundle;

        public bool AlreadyLoadAssets;

        public void Destroy(bool unload = true)
        {
            if (this.AssetBundle != null)
            {
                this.AssetBundle.Unload(unload);
            }

            this.Dispose();
        }
    }
    
    [ObjectSystem]
    public class ABInfoAwakeSystem: AwakeSystem<ABInfo, string, AssetBundle>
    {
        public override void Awake(ABInfo self, string abName, AssetBundle a)
        {
            self.AssetBundle = a;
            self.Name = abName;
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