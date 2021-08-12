using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [CreateAssetMenu(menuName = "SceneInfo", fileName ="DynamicObjectAsset" )]
    public class SceneDynamicObjectInfo : SerializedScriptableObject
    {
        public List<DynamicObjectInfo> DynamicObjectInfos = new List<DynamicObjectInfo>();
    }
}