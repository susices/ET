namespace ET
{
    public static class UnitHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = new UnitInfo();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitInfo.X = unit.Position.x;
            unitInfo.Y = unit.Position.y;
            unitInfo.Z = unit.Position.z;
            unitInfo.A = unit.Rotation.x;
            unitInfo.B = unit.Rotation.y;
            unitInfo.C = unit.Rotation.z;
            unitInfo.W = unit.Rotation.w;
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;

            foreach ((int key, long value) in nc.NumericDic)
            {
                unitInfo.Ks.Add(key);
                unitInfo.Vs.Add(value);
            }

            return unitInfo;
        }
    }
}