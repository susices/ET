using System.Collections;
using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;

namespace ET
{
    [CreateAssetMenu(menuName = "LocalizationText", fileName ="LocalizationTextAsset" )]
    public class LocalizationTextAsset : SerializedScriptableObject
    {
        [Button(name:"导入多语言文本")]
        public void LoadTextAsset()
        {
            string path = "Assets/Bundles/Config/LocalizationTextCategory.bytes";
            var data = File.OpenRead(path);
            var localizationCatgory =  ProtoBuf.Serializer.Deserialize<LocalizationTextCategory>(data);
            data.Close();
            Default.Clear();
            this.CN.Clear();
            this.EN.Clear();
            foreach (var localizationText in localizationCatgory.GetAll())
            {
                this.Default.Add(localizationText.Key,localizationText.Value.Default);
                this.CN.Add(localizationText.Key,localizationText.Value.CN);
                this.EN.Add(localizationText.Key,localizationText.Value.EN);
            }
        }

        public Dictionary<int, string> Default = new Dictionary<int, string>();

        public Dictionary<int, string> CN = new Dictionary<int, string>();
        
        public Dictionary<int, string> EN = new Dictionary<int, string>();
    }
}

