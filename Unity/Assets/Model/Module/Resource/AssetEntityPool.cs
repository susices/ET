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
        
        public string PrefabName;

        public string BundleName;

        public int RefCount { get; set; }

        public long DisposeTime;
    }

    public static class AssetEntityPoolSystem
    {
        /// <summary>
        /// 获取资源实体
        /// 外部调用只能调用这个方法！
        /// </summary>
        public static AssetEntity GetAssetEntity(this AssetEntityPool self, Transform parent = null)
        {
            var assetEntity = EntityFactory.Create<AssetEntity, AssetEntityPool, Transform>(self.Domain, self, parent, true);
            return assetEntity;
        }

        /// <summary>
        /// 取出实例化gameObject
        /// </summary>
        public static GameObject FetchGameObject(this AssetEntityPool self, Transform parent = null)
        {
            self.RefCount++;
            if (self.Pool.Count==0)
            {
                var obj = UnityEngine.Object.Instantiate(self.GameObjectRes, parent);
                return obj;
            }
            else
            {
                var obj =  self.Pool.Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(parent);
                return obj;
            }
        }

        /// <summary>
        /// 回收实例化gameObject
        /// </summary>
        public static void RecycleGameObject(this AssetEntityPool self, GameObject gameObject)
        {
            if (self==null)
            {
                return;
            }
            self.RefCount--;
            gameObject.transform.SetParent(PoolingAssetComponent.Instance.AssetPoolTransform);
            gameObject.SetActive(false);
            self.Pool.Enqueue(gameObject);
            if (self.RefCount==0)
            {
                // wenchao 待修改延迟销毁池的时间
                self.DisposeTime = TimeHelper.ClientNow() + 5000;
            }
        }
    }
    
    
    public class AssetEntityPoolAwakeSystem : AwakeSystem<AssetEntityPool, GameObject, string, string>
    {
        public override void Awake(AssetEntityPool self, GameObject gameObjectRes, string bundleName, string prefabName)
        {
            self.GameObjectRes = gameObjectRes;
            self.BundleName = bundleName;
            self.PrefabName = prefabName;
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
            if (ResourcesComponent.Instance==null)
            {
                return;
            }
            ResourcesComponent.Instance.UnloadBundle(self.BundleName);
            if (self.Parent is PoolingAssetComponent poolingAssetComponent)
            {
                poolingAssetComponent.PathAssetEntityPools.Remove(self.PrefabName);
            }
            else
            {
                Log.Error($"AssetEntityPool的父节点不是PoolingAssetComponent！");
            }
        }
    }
}