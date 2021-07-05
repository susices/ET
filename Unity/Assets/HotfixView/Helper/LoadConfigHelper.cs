using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ET
{
    public static class LoadConfigHelper
    {
        public static void LoadAllConfigBytes(Dictionary<string, byte[]> output)
        {
            Dictionary<string, UnityEngine.Object> keys = ResourcesComponent.Instance.GetBundleAll(AssetBundleHelper.ConfigDirPath);

            foreach (var kv in keys)
            {
                TextAsset v = kv.Value as TextAsset;
                string key = kv.Key;
                output[key] = v.bytes;
            }
        }
    }
}