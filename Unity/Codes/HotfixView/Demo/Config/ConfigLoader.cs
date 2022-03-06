using System.Collections.Generic;
using BM;
using UnityEngine;

namespace ET
{
    public class ConfigLoader: IConfigLoader
    {
        public void GetAllConfigBytes(Dictionary<string, byte[]> output)
        {
            //TODO
        }

        public byte[] GetOneConfigBytes(string configName)
        {
            var bytes =  AssetComponent.Load<TextAsset>($"Assets/Bundles/Config/{configName}.bytes").bytes;
            AssetComponent.UnLoadByPath($"Assets/Bundles/Config/{configName}.bytes");
            return bytes;
        }
    }
}