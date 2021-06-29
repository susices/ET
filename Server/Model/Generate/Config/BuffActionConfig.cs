using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class BuffActionConfigCategory : ProtoObject
    {
        public static BuffActionConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BuffActionConfig> dict = new Dictionary<int, BuffActionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BuffActionConfig> list = new List<BuffActionConfig>();
		
        public BuffActionConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (BuffActionConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public BuffActionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BuffActionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BuffActionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BuffActionConfig> GetAll()
        {
            return this.dict;
        }

        public BuffActionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BuffActionConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int BaseActionId { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public int[] actionArgs { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
