using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LocalizationUICategory : ProtoObject
    {
        public static LocalizationUICategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LocalizationUI> dict = new Dictionary<int, LocalizationUI>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LocalizationUI> list = new List<LocalizationUI>();
		
        public LocalizationUICategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (LocalizationUI config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public LocalizationUI Get(int id)
        {
            this.dict.TryGetValue(id, out LocalizationUI item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LocalizationUI)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LocalizationUI> GetAll()
        {
            return this.dict;
        }

        public LocalizationUI GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LocalizationUI: ProtoObject, IConfig
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
