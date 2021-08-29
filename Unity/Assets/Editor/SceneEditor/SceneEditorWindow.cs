using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ET;
using ProtoBuf;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using CharacterInfo = ET.CharacterInfo;
using Object = UnityEngine.Object;
using Unity.EditorCoroutines.Editor;

namespace ETEditor
{
    public class SceneEditorWindow:OdinEditorWindow
    {
        [TableList(ShowIndexLabels = false, AlwaysExpanded = true, IsReadOnly = true, DrawScrollView = true, HideToolbar = true)]
        public List<SceneEditItem> SceneEditItems = new List<SceneEditItem>();

        private GameObject sceneRoot
        {
            get
            {
                return GameObject.Find("SceneRoot");
            }
        }

        [ButtonGroup("SceneSelect")]
        public void SelectAll()
        {
            foreach (var sceneEditItem in this.SceneEditItems)
            {
                sceneEditItem.IsSelect = true;
            }
        }
        
        [ButtonGroup("SceneSelect")]
        public void SelectLoaded()
        {
            this.ClearSelect();
            foreach (var sceneEditItem in this.SceneEditItems)
            {
                if (sceneEditItem.IsLoaded)
                {
                    sceneEditItem.IsSelect = true;
                }
            }
        }
        
        [ButtonGroup("SceneSelect")]
        public void ClearSelect()
        {
            foreach (var sceneEditItem in this.SceneEditItems)
            {
                sceneEditItem.IsSelect = false;
            }
        }
        

        [EnumToggleButtons]
        public SceneEditType sceneEditType;

        [ButtonGroup("SceneEdit")]
        public void Load()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                if (sceneEditItem.IsLoaded)
                {
                    UnLoadSceneData(sceneEditItem);
                }
                LoadSceneData(sceneEditItem);
            }
            this.RefreshSceneDataItems();
        }
        
        
        
        [ButtonGroup("SceneEdit")]
        public void UnLoad()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                if (sceneEditItem.IsLoaded)
                {
                    UnLoadSceneData(sceneEditItem);
                }
            }
            this.RefreshSceneDataItems();
        }
        
        [ButtonGroup("SceneEdit")]
        public void Save()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                if (sceneEditItem.IsLoaded)
                {
                    SaveSceneData(sceneEditItem);
                }
            }
        }
        
        [ButtonGroup("SceneEdit")]
        public void Delete()
        {
            foreach (var sceneEditItem in this.GetSelectSceneEditItems())
            {
                if (sceneEditItem.IsLoaded)
                {
                    UnLoadSceneData(sceneEditItem);
                }
                DeleteSceneData(sceneEditItem);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            this.RefreshSceneDataItems();
        }

        public void LoadSceneData(SceneEditItem sceneEditItem)
        {
            List<SceneEntityManifest> sceneEntityManifests = new List<SceneEntityManifest>();
            string sceneName = sceneEditItem.SceneId.ToString();
            string path = $"{SceneEditorHelper.SceneDataDir}{SceneEditorHelper.SceneDataItemDirPre}{sceneName}/";
            var files = Directory.GetFiles(path).Where(x => !x.EndsWith("meta")).ToList();
            foreach (var filePath in files)
            {
                if (filePath.EndsWith("meta"))
                {
                    return;
                }
                using (var data = File.OpenRead(filePath))
                {
                    var manifest = Serializer.Deserialize<SceneEntityManifest>(data);
                    sceneEntityManifests.Add(manifest);
                }
            }
            SceneEditorHelper.LoadManifestBySceneType(sceneEntityManifests, this.sceneEditType, GameObject.Find("SceneRoot").transform);
        }
        
        public void UnLoadSceneData(SceneEditItem sceneEditItem)
        {
            if (this.sceneRoot==null)
            {
                return;
            }
            
            Transform sceneObjectTrans =  this.sceneRoot.transform.Find(sceneEditItem.SceneId.ToString());
            if (sceneObjectTrans!=null)
            {
                Editor.DestroyImmediate(sceneObjectTrans.gameObject);
            }
        }
        
        public void SaveSceneData(SceneEditItem sceneEditItem)
        {
            if (this.sceneRoot!=null)
            {
                var sceneDataTypes = ReflectionTools.GetImplementationsOf(typeof (ISceneEntityInfo));
                switch (sceneEditType)
                {
                    case SceneEditType.All:
                        foreach (var sceneDataType in sceneDataTypes)
                        {
                            SceneEditorHelper.SaveSceneData(sceneEditItem.SceneId, sceneDataType, this.sceneRoot.transform);
                        }
                        break;
                }
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
        }

        public void DeleteSceneData(SceneEditItem sceneEditItem)
        {
            string SceneDataItemDir = $"{SceneEditorHelper.SceneDataDir}SceneData{sceneEditItem.SceneId.ToString()}/";
            if (Directory.Exists(SceneDataItemDir))
            {
                AssetDatabase.DeleteAsset(SceneDataItemDir);
            }
        }

        public List<SceneEditItem> GetSelectSceneEditItems()
        {
            return SceneEditItems.Where(x => x.IsSelect == true).ToList();
        }
        
        public void RefreshSceneDataItems()
        {
            this.SceneEditItems.Clear();
            string[] dirs = Directory.GetDirectories(SceneEditorHelper.SceneDataDir);
            if ( dirs.Length == 0)
            {
                return;
            }

            foreach (var dir in dirs)
            {
                string subDir = dir.Substring(SceneEditorHelper.SceneDataDir.Length);
                if (subDir.StartsWith(SceneEditorHelper.SceneDataItemDirPre))
                {
                    try
                    {
                        int sceneId = int.Parse(subDir.Substring(SceneEditorHelper.SceneDataItemDirPre.Length));
                        SceneEditItem sceneEditItem = new SceneEditItem();
                        sceneEditItem.SceneId = sceneId;
                        this.SceneEditItems.Add(sceneEditItem);
                        if (this.sceneRoot!=null && this.sceneRoot.transform.Find(sceneEditItem.SceneId.ToString())!=null)
                        {
                            sceneEditItem.IsLoaded = true;
                        }
                        else
                        {
                            sceneEditItem.IsLoaded = false;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
                else
                {
                    Debug.Log("SceneData文件夹格式错误");
                }
            }
        }
        
    }
}

