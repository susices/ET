using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LocalizationTextCategory : ProtoObject
    {
        public static LocalizationTextCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LocalizationText> dict = new Dictionary<int, LocalizationText>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LocalizationText> list = new List<LocalizationText>();
		
        public LocalizationTextCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (LocalizationText config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public LocalizationText Get(int id)
        {
            this.dict.TryGetValue(id, out LocalizationText item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LocalizationText)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LocalizationText> GetAll()
        {
            return this.dict;
        }

        public LocalizationText GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LocalizationText: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public string CN { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public string EN { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
