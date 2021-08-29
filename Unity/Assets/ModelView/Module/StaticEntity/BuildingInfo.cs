using System;
using ProtoBuf;
using UnityEngine;

namespace ET
{
    [Serializable]
    [ProtoContract]
    public class BuildingInfo:ISceneEntityInfo
    {
        [ProtoMember(1)]
        public string path;
        
    }
}