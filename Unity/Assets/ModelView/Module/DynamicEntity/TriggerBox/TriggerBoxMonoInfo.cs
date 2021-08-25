using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(DynamicEntityMonoInfo))]
    public class TriggerBoxMonoInfo:MonoBehaviour
    {
        public TriggerBoxInfo TriggerBoxInfo = new TriggerBoxInfo();

        [OnValueChanged("OnTriggerBoxSizeChange")]
        public Vector3 TriggerBoxSize;

        public void OnTriggerBoxSizeChange()
        {
            this.TriggerBoxInfo.X = this.TriggerBoxSize.x;
            this.TriggerBoxInfo.Y = this.TriggerBoxSize.y;
            this.TriggerBoxInfo.Z = this.TriggerBoxSize.z;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(this.transform.position, this.TriggerBoxSize);
            
            Gizmos.color = Color.green/3;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(this.transform.position, this.TriggerBoxSize);
        }
    }
}