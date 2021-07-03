using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 资源实体池
    /// </summary>
    public class AssetEntityPool : Entity
    {
        public Queue<GameObject> Pool = new Queue<GameObject>();

        public GameObject GameObjectRes;

        public string BundleName;

        public int RefCount { get; set; }

        public long DisposeTime;
    }

    public static class AssetEntityPoolSystem
    {
        public static AssetEntity GetAssetEntity(this AssetEntityPool self)
        {
            return null;
        }

        public static GameObject FetchGameObject(this AssetEntityPool self)
        {
            self.RefCount++;
            if (self.Pool.Count==0)
            {
                var obj = UnityEngine.Object.Instantiate(self.GameObjectRes);
                return obj;
            }
            else
            {
                return self.Pool.Dequeue();
            }
        }

        public static void RecycleGameObject(this AssetEntityPool self, GameObject gameObject)
        {
            self.RefCount--;
            self.Pool.Enqueue(gameObject);
            if (self.RefCount==0)
            {
                self.DisposeTime = TimeHelper.ClientNow() + 10000;
            }
        }
    }
    
    
    public class AssetEntityPoolAwakeSystem : AwakeSystem<AssetEntityPool>
    {
        public override void Awake(AssetEntityPool self)
        {
            
        }
    }
    
    public class AssetEntityPoolUpdateSystem : UpdateSystem<AssetEntityPool>
    {
        public override void Update(AssetEntityPool self)
        {
            if (self.RefCount==0 && TimeHelper.ClientNow() > self.DisposeTime)
            {
                self.Dispose();
            }
        }
    }
    
    public class AssetEntityPoolDestroySystem : DestroySystem<AssetEntityPool>
    {
        public override void Destroy(AssetEntityPool self)
        {
            while (self.Pool.Count>0)
            {
                UnityEngine.Object.Destroy(self.Pool.Dequeue());
            }
            self.GameObjectRes = null;
            self.RefCount = 0;
            self.DisposeTime = 0;
            ResourcesComponent.Instance.UnloadBundle(self.BundleName);
        }
    }
}