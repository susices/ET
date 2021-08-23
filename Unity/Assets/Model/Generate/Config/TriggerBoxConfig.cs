using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class TriggerBoxConfigCategory : ProtoObject
    {
        public static TriggerBoxConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, TriggerBoxConfig> dict = new Dictionary<int, TriggerBoxConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<TriggerBoxConfig> list = new List<TriggerBoxConfig>();
		
        public TriggerBoxConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (TriggerBoxConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public TriggerBoxConfig Get(int id)
        {
            this.dict.TryGetValue(id, out TriggerBoxConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (TriggerBoxConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, TriggerBoxConfig> GetAll()
        {
            return this.dict;
        }

        public TriggerBoxConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class TriggerBoxConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int AssetIndex { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
