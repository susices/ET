using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class BaseActionConfigCategory : ProtoObject
    {
        public static BaseActionConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BaseActionConfig> dict = new Dictionary<int, BaseActionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BaseActionConfig> list = new List<BaseActionConfig>();
		
        public BaseActionConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (BaseActionConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public BaseActionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BaseActionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BaseActionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BaseActionConfig> GetAll()
        {
            return this.dict;
        }

        public BaseActionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BaseActionConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
