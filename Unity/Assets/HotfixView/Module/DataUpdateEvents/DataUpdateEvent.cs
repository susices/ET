namespace ET
{
    public class DataUpdateEvent: AEvent<EventType.DataUpdate>
    {
        protected override async ETTask Run(EventType.DataUpdate args)
        {
            if (!DataUpdateComponent.Instance.DataUpdateComponents.TryGetDic(args.DataType, out var dataUpdateComponentDic))
            {
                return;
            }

            if (args.DataType == DataType.BagItem)
            {
                foreach (var component in dataUpdateComponentDic.Values)
                {
                    if (component is UIBagComponent uiBagComponent)
                    {
                        uiBagComponent.OnDataUpdate();
                        continue;
                    }
                }
                return;
            }

            await ETTask.CompletedTask;
        }
    }
}