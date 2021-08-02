using System.ComponentModel.DataAnnotations;

namespace ET
{
    
    public class BagComponentDestorySystem:DestroySystem<BagComponent>
    {
        public override void Destroy(BagComponent self)
        {
            self.BagItems.Clear();
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
            
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Bag, self.PlayerId))
            {
                var bagItem = self.GetBagItem(bagItemId);
                if (bagItem==null || bagItem.DataValue< itemCount)
                {
                    return false;
                }
                await Game.Scene.GetComponent<DBComponent>().Save(self);
            }
            return true;
        }

        public static async ETTask<bool> AddItem(this BagComponent self, int bagItemId, int ItemCount)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Bag, self.PlayerId))
            {
                var bagItem = self.GetBagItem(bagItemId);
                if (bagItem==null)
                {
                    self.BagItems.Add(new BagItem()
                    {
                        DataId = bagItemId,
                        DataValue = ItemCount
                    });
                }
                else
                {
                    bagItem.DataValue += ItemCount;
                }

                return true;
            }
        }

        public static async ETTask<bool> DropItem(this BagComponent self, int bagItemId, int ItemCount)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Bag, self.PlayerId))
            {
                var bagItem = self.GetBagItem(bagItemId);
                if (bagItem==null || bagItem.DataValue<ItemCount)
                {
                    return false;
                }

                bagItem.DataValue -= ItemCount;
                return true;
            }
        }

        public static BagItem GetBagItem(this BagComponent self, int bagItemId)
        {
            for (int i = 0; i < self.BagItems.Count; i++)
            {
                if (self.BagItems[i].DataId == bagItemId)
                {
                    return self.BagItems[i];
                }
            }
            return null;
        }
        
        

    }
}