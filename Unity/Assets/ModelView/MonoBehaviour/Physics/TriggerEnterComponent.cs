using System;
using UnityEngine;

namespace ET
{
    public class TriggerEnterComponent : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEnterEvent;
        private void OnTriggerEnter(Collider other)
        {
            this.OnTriggerEnterEvent?.Invoke(other);
        }
    }
}