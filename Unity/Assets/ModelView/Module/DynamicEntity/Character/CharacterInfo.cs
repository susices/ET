using System;
using ProtoBuf;

namespace ET
{
    [Serializable]
    [ProtoContract]
    public class CharacterInfo:IDynamicEntityInfo
    {
        [ProtoMember(1)]
        public string Name;
    }
}