using System;
using ProtoBuf;

namespace ET
{
    
    [ProtoContract]
    public class TriggerBoxInfo:IDynamicEntityInfo
    {
        [ProtoMember(1)]
        public float X;
        [ProtoMember(2)]
        public float Y;
        [ProtoMember(3)]
        public float Z;
    }
}