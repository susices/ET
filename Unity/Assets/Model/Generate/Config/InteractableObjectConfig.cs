using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class InteractableObjectConfigCategory : ProtoObject
    {
        public static InteractableObjectConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, InteractableObjectConfig> dict = new Dictionary<int, InteractableObjectConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<InteractableObjectConfig> list = new List<InteractableObjectConfig>();
		
        public InteractableObjectConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (InteractableObjectConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public InteractableObjectConfig Get(int id)
        {
            this.dict.TryGetValue(id, out InteractableObjectConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (InteractableObjectConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, InteractableObjectConfig> GetAll()
        {
            return this.dict;
        }

        public InteractableObjectConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class InteractableObjectConfig: ProtoObject, IConfig
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
