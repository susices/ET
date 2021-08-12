using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LocalizationSceneCategory : ProtoObject
    {
        public static LocalizationSceneCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LocalizationScene> dict = new Dictionary<int, LocalizationScene>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LocalizationScene> list = new List<LocalizationScene>();
		
        public LocalizationSceneCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (LocalizationScene config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public LocalizationScene Get(int id)
        {
            this.dict.TryGetValue(id, out LocalizationScene item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LocalizationScene)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LocalizationScene> GetAll()
        {
            return this.dict;
        }

        public LocalizationScene GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LocalizationScene: ProtoObject, IConfig
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
