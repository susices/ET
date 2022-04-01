using ET.EventType;

namespace ET
{
    [NumericWatcher(NumericType.Strength)]
    [NumericWatcher(NumericType.Vitality)]
    [NumericWatcher(NumericType.Dexterity)]
    [NumericWatcher(NumericType.Spirit)]
    public class NumericWatcher_AddAttributePoint : INumericWatcher
    {
        public void Run(NumbericChange args)
        {
            if (!(args.Parent is Unit unit))
            {
                return;
            }

            switch (args.NumericType)
            {
                case NumericType.Strength:
                    unit.GetComponent<NumericComponent>()[NumericType.DamageAddValue]+=5;
                    break;
                case NumericType.Vitality:
                    unit.GetComponent<NumericComponent>()[NumericType.HpPct] += 1*10000;
                    break;
                case NumericType.Dexterity:
                    unit.GetComponent<NumericComponent>()[NumericType.DefenseValueFinalAdd] += 5;
                    break;
                case NumericType.Spirit:
                    unit.GetComponent<NumericComponent>()[NumericType.MpFinalPct] += 1*10000;
                    break;
            }
        }
    }
}