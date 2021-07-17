using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class UIPanelConfigCategory : ProtoObject
    {
        public static UIPanelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, UIPanelConfig> dict = new Dictionary<int, UIPanelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<UIPanelConfig> list = new List<UIPanelConfig>();
		
        public UIPanelConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (UIPanelConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public UIPanelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out UIPanelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (UIPanelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, UIPanelConfig> GetAll()
        {
            return this.dict;
        }

        public UIPanelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class UIPanelConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int[] UIPanelComponentIndexs { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int AssetPath { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public int UILayer { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
