using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class SceneEntityManifest:ProtoObject
    {
        [ProtoMember(1)]
        public List<SceneEntityInfo> list = new List<SceneEntityInfo>();
    }

    [ProtoContract]
    public class SceneEntityInfo: ProtoObject
    {
        [ProtoMember(1)]
        public int EntityId;
        
        [ProtoMember(2)]
        public int SceneId;

        [ProtoMember(3)]
        public int DynamicEntityConfigId;

        [ProtoMember(100)]
        public float PositionX;

        [ProtoMember(101)]
        public float PositionY;

        [ProtoMember(102)]
        public float PositionZ;

        [ProtoMember(103)]
        public float ScaleX;
        
        [ProtoMember(104)]
        public float ScaleY;
        
        [ProtoMember(105)]
        public float ScaleZ;

        [ProtoMember(106)]
        public float RotationX;
        
        [ProtoMember(107)]
        public float RotationY;
        
        [ProtoMember(108)]
        public float RotationZ;
        
        [ProtoMember(109)]
        public float RotationW;
    }
}