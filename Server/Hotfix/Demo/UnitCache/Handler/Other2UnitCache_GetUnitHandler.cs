using System;
using System.Collections.Generic;

namespace ET
{
    public class Other2UnitCache_GetUnitHandler:AMActorRpcHandler<Scene,Other2UnitCache_GetUnit, UnitCache2Other_GetUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_GetUnit request, UnitCache2Other_GetUnit response, Action reply)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            Dictionary<string, Entity> dictionary = MonoPool.Instance.Fetch(typeof (Dictionary<string, Entity>)) as Dictionary<string, Entity>;
            try
            {
                if (request.ComponentNameList.Count == 0)
                {
                    dictionary.Add(nameof (Unit), null);
                    foreach (var key in unitCacheComponent.UnitKeyList)
                    {
                        dictionary.Add(key, null);
                    }
                }
                else
                {
                    foreach (var component in request.ComponentNameList)
                    {
                        dictionary.Add(component, null);
                    }
                }

                foreach (var key in dictionary.Keys)
                {
                    Entity entity = await unitCacheComponent.Get(request.UnitId, key);
                    dictionary[key] = entity;
                }

                response.ComponentNameList.AddRange(dictionary.Keys);
                response.EntityList.AddRange(dictionary.Values);
            }
            finally
            {
                dictionary.Clear();
                MonoPool.Instance.Recycle(dictionary);
            }
            reply();
        }
    }
}