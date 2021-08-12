using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LocalizationModelCategory : ProtoObject
    {
        public static LocalizationModelCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LocalizationModel> dict = new Dictionary<int, LocalizationModel>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LocalizationModel> list = new List<LocalizationModel>();
		
        public LocalizationModelCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (LocalizationModel config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public LocalizationModel Get(int id)
        {
            this.dict.TryGetValue(id, out LocalizationModel item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LocalizationModel)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LocalizationModel> GetAll()
        {
            return this.dict;
        }

        public LocalizationModel GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LocalizationModel: ProtoObject, IConfig
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
		public int CachePoolMillSeconds { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
