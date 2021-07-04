using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LocalizationAudioCategory : ProtoObject
    {
        public static LocalizationAudioCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LocalizationAudio> dict = new Dictionary<int, LocalizationAudio>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LocalizationAudio> list = new List<LocalizationAudio>();
		
        public LocalizationAudioCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (LocalizationAudio config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public LocalizationAudio Get(int id)
        {
            this.dict.TryGetValue(id, out LocalizationAudio item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LocalizationAudio)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LocalizationAudio> GetAll()
        {
            return this.dict;
        }

        public LocalizationAudio GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LocalizationAudio: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public string Default { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public string CN { get; set; }
		[ProtoMember(5, IsRequired  = true)]
		public string EN { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
