using System.Linq;

namespace ET
{
    public class M2C_RemoveUnitsHandler : AMHandler<M2C_RemoveUnits>
    {
        protected override async ETVoid Run(Session session, M2C_RemoveUnits message)
        {
            UnitComponent unitComponent = session.DomainScene().GetComponent<UnitComponent>();
            for (int i = 0; i < message.UnitIds.Count; i++)
            {
                //处理移除其他玩家unit
                if (message.UnitIds[i] != unitComponent.MyUnit.Id)
                {
                    unitComponent.Remove(message.UnitIds[i]);
                }
                //传送时移除 除自身的所有unit
                else
                {
                    var allUnits = unitComponent.idUnits.Keys.ToList();
                    foreach (var unitId in allUnits)
                    {
                        if (unitId!=unitComponent.MyUnit.Id)
                        {
                            unitComponent.Remove(unitId);
                        }
                    }
                    break;
                }
            }
            await ETTask.CompletedTask;
        }
    }
}