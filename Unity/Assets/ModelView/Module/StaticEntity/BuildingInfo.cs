using System;
using ProtoBuf;
using UnityEngine;

namespace ET
{
    [Serializable]
    [ProtoContract]
    [SceneEntityInfo]
    public class BuildingInfo:ISceneEntityInfo
    {
        [ProtoMember(1)]
        public string path;
    }
}