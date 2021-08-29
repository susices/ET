using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ET;
using ProtoBuf;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Slate;
using UnityEditor;
using UnityEngine;
using CharacterInfo = ET.CharacterInfo;
using Path = System.IO.Path;

namespace ETEditor
{
    public class SceneCreateEditorWindow : OdinEditorWindow
    {
        
        public int SceneId;
        
        [Button("新建场景文件")]
        public void CreateNewScene()
        {
            string SceneDataItemDir = $"{SceneEditorHelper.SceneDataDir}SceneData{this.SceneId.ToString()}/";
            if (Directory.Exists(SceneDataItemDir))
            {
                Directory.Delete(SceneDataItemDir, true);
            }
            Directory.CreateDirectory(SceneDataItemDir);
            Type[] types = ReflectionTools.GetImplementationsOf(typeof (ISceneEntityInfo));
            foreach (Type type in types)
            {
                string path = Path.Combine(SceneDataItemDir, $"{type.Name}.bytes");
                SceneEntityManifest manifest = new SceneEntityManifest();
                manifest.SceneId = this.SceneId;
                if (type == typeof(BuildingInfo))
                {
                    manifest.list.Add(new SceneEntityBuildInfo(){Position = new float[]{1,2,3}, Scale = new float[]{2,2,2}, Rotation = new float[]{0,0,0,0},  SceneEntityInfo = new BuildingInfo(){path = "Assets/Bundles/Cube.prefab"}});
                    manifest.list.Add(new SceneEntityBuildInfo(){Position = new float[]{10,20,30}, Scale = new float[]{1,1,1}, Rotation = new float[]{0,0,0,0}, SceneEntityInfo = new BuildingInfo(){path = "Assets/Bundles/Cube.prefab"}});
                }
                using (FileStream file = File.Create(path))
                {
                    Serializer.Serialize(file, manifest);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

