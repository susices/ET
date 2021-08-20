using System;
using Sirenix.OdinInspector;
using Slate;
using UnityEditor;
using UnityEngine;

namespace ET
{
    [DynamicActor]
    public class DynamicActorWrapper:MonoBehaviour, IDynamicActor
    {
        [OnValueChanged("SetName")]
        public int ActorId;
        
        public GameObject GetActor()
        {
            return GameObject.Find(this.ActorId.ToString());
        }

        private void SetName()
        {
            this.gameObject.name = $"Dynamic Actor:{this.ActorId.ToString()}";
        }
    }
}