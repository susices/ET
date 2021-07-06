using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LocalizationArtAssetCategory : ProtoObject
    {
        public static LocalizationArtAssetCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LocalizationArtAsset> dict = new Dictionary<int, LocalizationArtAsset>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LocalizationArtAsset> list = new List<LocalizationArtAsset>();
		
        public LocalizationArtAssetCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (LocalizationArtAsset config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public LocalizationArtAsset Get(int id)
        {
            this.dict.TryGetValue(id, out LocalizationArtAsset item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LocalizationArtAsset)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LocalizationArtAsset> GetAll()
        {
            return this.dict;
        }

        public LocalizationArtAsset GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LocalizationArtAsset: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public string Default { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public string CN { get; set; }
		[ProtoMember(5, IsRequired  = true)]
		public string EN { get; set; }
		[ProtoMember(6, IsRequired  = true)]
		public int CachePoolSeconds { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
