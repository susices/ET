using System;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [ProtoInclude(20,typeof(CharacterInfo))]
    [ProtoInclude(21,typeof(InteractionInfo))]
    [ProtoInclude(22,typeof(PickableInfo))]
    [ProtoInclude(23,typeof(TriggerBoxInfo))]
    [ProtoInclude(24,typeof(BuildingInfo))]
    public interface ISceneEntityInfo
    {
        
    }
}