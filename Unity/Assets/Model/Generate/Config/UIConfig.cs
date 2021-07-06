using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class UIConfigCategory : ProtoObject
    {
        public static UIConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, UIConfig> dict = new Dictionary<int, UIConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<UIConfig> list = new List<UIConfig>();
		
        public UIConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (UIConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public UIConfig Get(int id)
        {
            this.dict.TryGetValue(id, out UIConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (UIConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, UIConfig> GetAll()
        {
            return this.dict;
        }

        public UIConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class UIConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public string AssetPath { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public int UILayer { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
