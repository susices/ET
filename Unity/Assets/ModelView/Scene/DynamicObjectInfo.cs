using System;
using UnityEngine;

namespace ET
{
    [Serializable]
    public class DynamicObjectInfo
    {
        public int ConfigId;
        
        public Vector3 Position;

        public Quaternion Rotation;

        public Vector3 Scale;
    }
}