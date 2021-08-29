using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace ET
{
    [ProtoContract]
    public class SceneEntityManifest:ProtoObject
    {
        [ProtoMember(1)]
        public int SceneId;
        
        [ProtoMember(2)]
        public List<SceneEntityBuildInfo> list = new List<SceneEntityBuildInfo>();
    }

    [ProtoContract]
    public class SceneEntityBuildInfo: ProtoObject
    {
        [ProtoMember(2)]
        public ISceneEntityInfo SceneEntityInfo;

        [ProtoMember(100)]
        public float[] Position;

        [ProtoMember(101)]
        public float[] Scale;

        [ProtoMember(102)]
        public float[] Rotation;



        public Vector3 GetPosition()
        {
            return new Vector3(this.Position[0], this.Position[1], this.Position[2]);
        }

        public Vector3 GetScale()
        {
            return new Vector3(this.Scale[0], this.Scale[1], this.Scale[2]);
        }

        public Quaternion GetRotation()
        {
            return new Quaternion(this.Rotation[0], this.Rotation[1], this.Rotation[2], this.Rotation[3]);
        }
    }
}