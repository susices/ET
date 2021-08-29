using System;
using ProtoBuf;
using UnityEngine;

namespace ET
{
    [Serializable]
    [ProtoContract]
    [SceneEntityInfo]
    public class CharacterInfo:ISceneEntityInfo
    {
        [ProtoMember(1)]
        public string Name;
    }
}