using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class InteractableConfigCategory : ProtoObject
    {
        public static InteractableConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, InteractableConfig> dict = new Dictionary<int, InteractableConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<InteractableConfig> list = new List<InteractableConfig>();
		
        public InteractableConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (InteractableConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public InteractableConfig Get(int id)
        {
            this.dict.TryGetValue(id, out InteractableConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (InteractableConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, InteractableConfig> GetAll()
        {
            return this.dict;
        }

        public InteractableConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class InteractableConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int AssetIndex { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int[] InteractBuffId { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
