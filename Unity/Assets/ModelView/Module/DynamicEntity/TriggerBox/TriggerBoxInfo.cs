using System;
using ProtoBuf;
using UnityEngine;

namespace ET
{
    
    [ProtoContract]
    [SceneEntityInfo]
    public class TriggerBoxInfo:ISceneEntityInfo
    {
        [ProtoMember(1)]
        public float X;
        [ProtoMember(2)]
        public float Y;
        [ProtoMember(3)]
        public float Z;
        
    }
}