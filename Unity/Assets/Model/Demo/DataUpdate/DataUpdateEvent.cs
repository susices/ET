﻿using System;
using System.Collections.Generic;

namespace ET
{
    public class DataUpdateEvent:AEvent<EventType.DataUpdate>
    {
        protected override async ETTask Run(EventType.DataUpdate dataUpdate)
        {
            await DataUpdateComponent.Instance.Run(dataUpdate.DataUpdateType, dataUpdate.ComponentId);
        }
    }
}