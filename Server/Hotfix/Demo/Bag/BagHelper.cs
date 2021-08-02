namespace ET
{
    public static class BagHelper
    {
        public static async ETTask AddBagComponent(Unit unit)
        {
            var bagComponents =  await Game.Scene.GetComponent<DBComponent>()
                    .Query<BagComponent>(d => d.PlayerId == unit.GetComponent<UnitInfoComponent>().PlayerId);
            if (bagComponents.Count!=1)
            {
                Log.Error("未找到玩家背包组件数据");
                return;
            }
            unit.AddComponent(bagComponents[0]);
        }
    }
}