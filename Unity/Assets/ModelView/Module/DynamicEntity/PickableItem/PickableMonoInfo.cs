using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(DynamicEntityMonoInfo))]
    public class PickableMonoInfo:MonoBehaviour
    {
        public PickableInfo PickableInfo;
    }
}