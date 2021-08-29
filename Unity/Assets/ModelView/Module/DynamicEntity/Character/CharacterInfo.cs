using System;
using ProtoBuf;
using UnityEngine;

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