using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bright.Serialization;

namespace ET
{
	[ObjectSystem]
    public class ConfigAwakeSystem : AwakeSystem<ConfigComponent>
    {
        public override void Awake(ConfigComponent self)
        {
	        ConfigComponent.Instance = self;
        }
    }
    
    [ObjectSystem]
    public class ConfigDestroySystem : DestroySystem<ConfigComponent>
    {
	    public override void Destroy(ConfigComponent self)
	    {
		    ConfigComponent.Instance = null;
	    }
    }
    
    public static class ConfigComponentSystem
	{
		public static void LoadOneConfig(this ConfigComponent self, Type configType)
		{
			// TODO
		}
		
		public static void Load(this ConfigComponent self)
		{
			self.Tables = new Tables(self.loadByteBuf);
		}

		private static ByteBuf loadByteBuf(this ConfigComponent self, string tableName)
		{
			return new ByteBuf(self.ConfigLoader.GetOneConfigBytes(tableName));
		}
	}
}