using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class UnitySceneConfigCategory : ProtoObject
    {
        public static UnitySceneConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, UnitySceneConfig> dict = new Dictionary<int, UnitySceneConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<UnitySceneConfig> list = new List<UnitySceneConfig>();
		
        public UnitySceneConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (UnitySceneConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public UnitySceneConfig Get(int id)
        {
            this.dict.TryGetValue(id, out UnitySceneConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (UnitySceneConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, UnitySceneConfig> GetAll()
        {
            return this.dict;
        }

        public UnitySceneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class UnitySceneConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public string SceneName { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public int AssetPath { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
