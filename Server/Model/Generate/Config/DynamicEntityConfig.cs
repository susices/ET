using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class DynamicEntityConfigCategory : ProtoObject
    {
        public static DynamicEntityConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DynamicEntityConfig> dict = new Dictionary<int, DynamicEntityConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DynamicEntityConfig> list = new List<DynamicEntityConfig>();
		
        public DynamicEntityConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (DynamicEntityConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public DynamicEntityConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DynamicEntityConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DynamicEntityConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DynamicEntityConfig> GetAll()
        {
            return this.dict;
        }

        public DynamicEntityConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DynamicEntityConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int AssetIndex { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int[] AttachBuffs { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
