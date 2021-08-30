namespace ET
{
    public static class InteractionComponentSystem
    {
        /// <summary>
        /// 执行交互
        /// </summary>
        public static void Interaction(this InteractionComponent self, Entity sourceEntity)
        {
            var buffContainer = sourceEntity.GetComponent<BuffContainerComponent>();
            if (buffContainer==null)
            {
                return;
            }

            var InteractBuffIds =  InteractableConfigCategory.Instance.Get(self.InteractionConfigId).InteractBuffId;
            if (InteractBuffIds==null)
            {
                return;
            }
            
            foreach (var buffId in InteractBuffIds)
            {
                buffContainer.TryAddBuff(buffId, sourceEntity);
            }
        }
    }
}