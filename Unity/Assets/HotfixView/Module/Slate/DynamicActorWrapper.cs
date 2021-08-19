using System;
using UnityEngine;

namespace ET
{
    public class DynamicActorWrapper:MonoBehaviour
    {
        public string ActorName;

        public GameObject GetActor()
        {
            return GameObject.Find(this.ActorName);
        }
    }
}