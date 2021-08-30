using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class QuestTargetConfigCategory : ProtoObject
    {
        public static QuestTargetConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, QuestTargetConfig> dict = new Dictionary<int, QuestTargetConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<QuestTargetConfig> list = new List<QuestTargetConfig>();
		
        public QuestTargetConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (QuestTargetConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public QuestTargetConfig Get(int id)
        {
            this.dict.TryGetValue(id, out QuestTargetConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (QuestTargetConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, QuestTargetConfig> GetAll()
        {
            return this.dict;
        }

        public QuestTargetConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class QuestTargetConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int Title { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int TargetType { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public int[] ReceiveBuffId { get; set; }
		[ProtoMember(5, IsRequired  = true)]
		public int[] CompleteBuffId { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
