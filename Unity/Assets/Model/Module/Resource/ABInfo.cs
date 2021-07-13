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
}