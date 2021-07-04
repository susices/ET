using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{

    [ProtoContract]
    public class AssetManifest : ProtoObject
    {

        public static AssetManifest Instance;

        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<string, AssetInfo> dict = new Dictionary<string, AssetInfo>();

        [BsonElement]
        [ProtoMember(1)]
        public List<AssetInfo> list = new List<AssetInfo>();
        
        public AssetManifest()
        {
            Instance = this;
        }
        
        [ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (var assetInfo in this.list)
            {
                this.dict.Add(assetInfo.AssetPath, assetInfo);
            }
            this.list.Clear();
            this.EndInit();
        }

        public AssetInfo Get(string assetPath)
        {
            if (this.dict.TryGetValue(assetPath, out var assetInfo))
            {
                return assetInfo;
            }
            else
            {
                return null;
            }
        }
    }


    [ProtoContract]
    public class AssetInfo : ProtoObject
    {
        [ProtoMember(1)]
        public string AssetPath { get; set;}

        [ProtoMember(2)]
        public string PrefabName { get; set; }

        [ProtoMember(3)]
        public string BundleName { get; set; }
        
        [ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
    }
}