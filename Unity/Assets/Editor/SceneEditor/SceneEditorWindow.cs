using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ET;
using ProtoBuf;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    public class SceneEditorWindow: OdinEditorWindow
    {
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true)]
        public List<SceneEditItem> SceneEditItems = new List<SceneEditItem>();

        [ButtonGroup("Load")]
        [Button("加载动态实体")]
        public void LoadDynamicEntity()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }

        [ButtonGroup("Load")]
        [Button("加载静态实体")]
        public void LoadStaticEntity()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }

        [ButtonGroup("Load")]
        [Button("加载全部实体")]
        public void LoadAllEntity()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }
        
        [ButtonGroup("UnLoad")]
        [Button("卸载动态实体")]
        public void UnLoadDynamicEntity()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }

        [ButtonGroup("UnLoad")]
        [Button("卸载静态实体")]
        public void UnLoadStaticEntity()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }

        [ButtonGroup("UnLoad")]
        [Button("卸载全部实体")]
        public void UnLoadAllEntity()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }
        
        
        

        [MenuItem("Tools/场景编辑器窗口")]
        public static void PopUp()
        {
            SceneEditorWindow sceneEditorWindow = GetWindow<SceneEditorWindow>();
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
        }

        
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

        
        public static void LoadSceneData()
        {
            
        }


        public List<SceneEditItem> GetSelectSceneEditItems()
        {
            return SceneEditItems.Where(x => x.IsSelect == true).ToList();
        }
    }
}

