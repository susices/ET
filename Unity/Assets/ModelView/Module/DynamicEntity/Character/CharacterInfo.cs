using System;
using ProtoBuf;

namespace ET
{
    [Serializable]
    [ProtoContract]
    public class CharacterInfo:ISceneEntityInfo
    {
        [ProtoMember(1)]
        public string Name;
    }
}