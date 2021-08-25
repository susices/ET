using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class SceneEntityManifest:ProtoObject
    {
        [ProtoMember(1)]
        public int SceneId;
        
        [ProtoMember(2)]
        public List<SceneEntityInfo> list = new List<SceneEntityInfo>();
    }

    [ProtoContract]
    public class SceneEntityInfo: ProtoObject
    {
        [ProtoMember(1)]
        public int DynamicEntityConfigId;

        [ProtoMember(2)]
        public IDynamicEntityInfo DynamicEntityInfo;

        [ProtoMember(100)]
        public float[] Position;

        [ProtoMember(101)]
        public float[] Scale;

        [ProtoMember(102)]
        public float[] Rotation;
    }
}