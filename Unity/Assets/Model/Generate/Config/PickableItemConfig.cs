using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class PickableItemConfigCategory : ProtoObject
    {
        public static PickableItemConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PickableItemConfig> dict = new Dictionary<int, PickableItemConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PickableItemConfig> list = new List<PickableItemConfig>();
		
        public PickableItemConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (PickableItemConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public PickableItemConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PickableItemConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PickableItemConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PickableItemConfig> GetAll()
        {
            return this.dict;
        }

        public PickableItemConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PickableItemConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int AssetIndex { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int[] PickItemBuffId { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public int[] UseItemBuffId { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
