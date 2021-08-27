using System;
using ProtoBuf;

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