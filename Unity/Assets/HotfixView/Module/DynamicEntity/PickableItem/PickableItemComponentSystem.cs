namespace ET
{
    public static class PickableItemComponentSystem
    {
        /// <summary>
        /// 执行物品拾取
        /// </summary>
        public static void PickItem(this PickableItemComponent self, Entity sourceEntity)
        {
            var buffContainer = sourceEntity.GetComponent<BuffContainerComponent>();
            if (buffContainer==null)
            {
                return;
            }

            var PickItemBuffIds =  PickableItemConfigCategory.Instance.Get(self.PickableConfigId).PickItemBuffId;
            if (PickItemBuffIds==null)
            {
                return;
            }
            
            foreach (var buffId in PickItemBuffIds)
            {
                buffContainer.TryAddBuff(buffId, sourceEntity);
            }
        }
    }
}