using System.Collections.Generic;

namespace ET
{
    public partial class MapNavMeshConfig
    {
        
    }
    
    public partial class MapNavMeshConfigCategory
    {
        public Dictionary<string, MapNavMeshConfig> Maps = new Dictionary<string, MapNavMeshConfig>();

        public override void EndInit()
        {
            foreach (MapNavMeshConfig mapNavMeshConfig in Instance.GetAll().Values)
            {
                this.Maps.Add(mapNavMeshConfig.MapName, mapNavMeshConfig);
            }
        }
    }
}