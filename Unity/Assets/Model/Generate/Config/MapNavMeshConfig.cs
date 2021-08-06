using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MapNavMeshConfigCategory : ProtoObject
    {
        public static MapNavMeshConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapNavMeshConfig> dict = new Dictionary<int, MapNavMeshConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapNavMeshConfig> list = new List<MapNavMeshConfig>();
		
        public MapNavMeshConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (MapNavMeshConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public MapNavMeshConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MapNavMeshConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MapNavMeshConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapNavMeshConfig> GetAll()
        {
            return this.dict;
        }

        public MapNavMeshConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapNavMeshConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public string MapName { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public int UnitySceneAssetPath { get; set; }
		[ProtoMember(5, IsRequired  = true)]
		public string NavMeshPath { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
