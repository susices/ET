using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class FrameworkConfigCategory : ProtoObject
    {
        public static FrameworkConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FrameworkConfig> dict = new Dictionary<int, FrameworkConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FrameworkConfig> list = new List<FrameworkConfig>();
		
        public FrameworkConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (FrameworkConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public FrameworkConfig Get(int id)
        {
            this.dict.TryGetValue(id, out FrameworkConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FrameworkConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FrameworkConfig> GetAll()
        {
            return this.dict;
        }

        public FrameworkConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FrameworkConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int IntVar { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public string StringVar { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
