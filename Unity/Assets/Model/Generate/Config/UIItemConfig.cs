using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class UIItemConfigCategory : ProtoObject
    {
        public static UIItemConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, UIItemConfig> dict = new Dictionary<int, UIItemConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<UIItemConfig> list = new List<UIItemConfig>();
		
        public UIItemConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (UIItemConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public UIItemConfig Get(int id)
        {
            this.dict.TryGetValue(id, out UIItemConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (UIItemConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, UIItemConfig> GetAll()
        {
            return this.dict;
        }

        public UIItemConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class UIItemConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int AssetPath { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
