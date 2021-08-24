using System.Collections;
using System.Collections.Generic;
using System.IO;
using ET;
using ProtoBuf;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    public class SceneEditor
    {
        [MenuItem("Tools/保存场景")]
        public static void SaveSceneData()
        {
            string sceneName = "1001";
            string path = Path.Combine("Assets/Bundles/SceneEntity/", $"{sceneName}.bytes");
            SceneEntityManifest manifest = new SceneEntityManifest();
            
            using FileStream file = File.Create(path);
            Serializer.Serialize(file, manifest);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/读取场景")]
        public static void LoadSceneData()
        {
            
        }
    }
}

