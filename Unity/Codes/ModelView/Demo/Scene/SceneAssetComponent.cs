using System.Collections.Generic;
using BM;

namespace ET
{
    public class SceneAssetComponent: Entity,IAwake
    {
        public Dictionary<string, LoadSceneHandler> LoadSceneHandlers = new Dictionary<string, LoadSceneHandler>();
    }
}