using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class CharacterConfigCategory : ProtoObject
    {
        public static CharacterConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, CharacterConfig> dict = new Dictionary<int, CharacterConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<CharacterConfig> list = new List<CharacterConfig>();
		
        public CharacterConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (CharacterConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public CharacterConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CharacterConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (CharacterConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CharacterConfig> GetAll()
        {
            return this.dict;
        }

        public CharacterConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class CharacterConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int AssetIndex { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int[] InteractBuffId { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
