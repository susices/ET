using UnityEditor;
using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(DynamicEntityMonoInfo))]
    public class InteractionMonoInfo:MonoBehaviour
    {
        public InteractionInfo InteractionInfo;
    }
}