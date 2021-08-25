using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    
    public class DynamicEntityMonoInfo : MonoBehaviour
    {
        [ReadOnly]
        public DynamicEntityType DynamicEntityType;
        
        [ReadOnly]
        public int DynamicEntityConfigId;
    }
}

