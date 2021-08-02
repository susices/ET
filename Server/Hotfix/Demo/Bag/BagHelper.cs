namespace ET
{
    public static class BagHelper
    {
        public static async ETTask AddBagComponent(Unit unit)
        {
            var bagInfos =  await Game.Scene.GetComponent<DBComponent>()
                    .Query<BagInfo>(d => d.PlayerId == unit.GetComponent<UnitInfoComponent>().PlayerId);
            if (bagInfos.Count!=1)
            {
                Log.Error("未找到玩家背包组件数据");
                return;
            }
            unit.AddComponent<BagComponent,BagInfo>(bagInfos[0]);
        }
    }
}