using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class DynamicEntityInfo : MonoBehaviour
    {
        [ReadOnly]
        public DynamicEntityType DynamicEntityType;
        [ReadOnly]
        public int DynamicEntityConfigId;
        [ReadOnly]
        public int EntityId;
    }
} 

