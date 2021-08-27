using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    
    public class DynamicEntityMonoInfo : MonoBehaviour
    {
        [ReadOnly]
        public SceneEntityType sceneEntityType;
        
        [ReadOnly]
        public int DynamicEntityConfigId;
    }
}

