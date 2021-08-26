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
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true, IsReadOnly = true)]
        public List<SceneEditItem> SceneEditItems = new List<SceneEditItem>();

        [ToggleLeft]
        [OnValueChanged("OnSelectAllClick")]
        public bool SelectAll;
        
        [EnumToggleButtons]
        public SceneEntityType sceneEntityType;

        [ButtonGroup("SceneEdit")]
        public void Load()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }
        
        [ButtonGroup("SceneEdit")]
        public void UnLoad()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }
        
        [ButtonGroup("SceneEdit")]
        public void Save()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                
            }
        }
        
        [ButtonGroup("SceneEdit")]
        public void Delete()
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
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
            sceneEditorWindow.SceneEditItems.Add(new SceneEditItem());
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

        public void OnSelectAllClick()
        {
            foreach (var sceneEditItem in SceneEditItems)
            {
                sceneEditItem.IsSelect = SelectAll;
            }
        }


        public List<SceneEditItem> GetSelectSceneEditItems()
        {
            return SceneEditItems.Where(x => x.IsSelect == true).ToList();
        }
    }
}

