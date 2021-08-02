using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class BagComponentAwakeSystem:AwakeSystem<BagComponent, BagInfo>
    {
        public override void Awake(BagComponent self,BagInfo bagInfo)
        {
            self.BagInfo = bagInfo;
        }
    }
    
    public class BagComponentDestorySystem:DestroySystem<BagComponent>
    {
        public override void Destroy(BagComponent self)
        {
            self.BagInfo = null;
        }
    }

    public static class BagComponentSystem
    {
        public static async ETTask<bool> UseBagItem(this BagComponent self, int bagItemId, int itemCount)
        {
            if (itemCount==0)
            {
                return false;
            }
            
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Bag, self.BagInfo.PlayerId))
            {
                var bagItem = self.GetBagItem(bagItemId);
                if (bagItem==null || bagItem.DataValue< itemCount)
                {
                    return false;
                }
                bagItem.DataValue -= itemCount;
                await Game.Scene.GetComponent<DBComponent>().Save(self.BagInfo);
            }
            return true;
        }

        public static async ETTask<bool> AddItem(this BagComponent self, int bagItemId, int ItemCount)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Bag, self.BagInfo.PlayerId))
            {
                var bagItem = self.GetBagItem(bagItemId);
                if (bagItem==null)
                {
                    self.BagInfo.BagItems.Add(new BagItem()
                    {
                        DataId = bagItemId,
                        DataValue = ItemCount
                    });
                }
                else
                {
                    bagItem.DataValue += ItemCount;
                }
                await Game.Scene.GetComponent<DBComponent>().Save(self.BagInfo);
                return true;
            }
        }

        public static async ETTask<bool> DropItem(this BagComponent self, int bagItemId, int ItemCount)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Bag, self.BagInfo.PlayerId))
            {
                var bagItem = self.GetBagItem(bagItemId);
                if (bagItem==null || bagItem.DataValue<ItemCount)
                {
                    return false;
                }
                bagItem.DataValue -= ItemCount;
                await Game.Scene.GetComponent<DBComponent>().Save(self.BagInfo);
                return true;
            }
        }

        public static BagItem GetBagItem(this BagComponent self, int bagItemId)
        {
            for (int i = 0; i < self.BagInfo.BagItems.Count; i++)
            {
                if (self.BagInfo.BagItems[i].DataId == bagItemId)
                {
                    return self.BagInfo.BagItems[i];
                }
            }
            return null;
        }
    }
}