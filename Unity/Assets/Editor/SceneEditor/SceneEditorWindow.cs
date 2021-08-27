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
using UnityEditor.Search;
using UnityEngine;
using CharacterInfo = ET.CharacterInfo;
using Object = UnityEngine.Object;

namespace ETEditor
{
    public class SceneEditorWindow:OdinEditorWindow
    {
        private const string SceneDataDir = "Assets/Bundles/SceneConfigData/";
        private const string SceneDataItemDirPre = "SceneData";
        [InlineButton("CreateNewScene")]
        
        public int SceneId;
        
        [TableList(ShowIndexLabels = false, AlwaysExpanded = true, IsReadOnly = true, DrawScrollView = true, HideToolbar = true)]
        public List<SceneEditItem> SceneEditItems = new List<SceneEditItem>();
        private GameObject sceneRoot;

        private void OnBecameVisible()
        {
            this.sceneRoot = GameObject.Find("SceneRoot");
            this.RefreshSceneDataItems();
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
        }
        
        
        public void CreateNewScene()
        {
            string SceneDataItemDir = $"{SceneDataDir}SceneData{this.SceneId.ToString()}/";
            if (Directory.Exists(SceneDataItemDir))
            {
                Directory.Delete(SceneDataItemDir, true);
            }
            Directory.CreateDirectory(SceneDataItemDir);
            
            foreach (Type type in new Type[]
            {
                typeof(CharacterInfo),
                typeof(InteractionInfo),
                typeof(PickableInfo),
                typeof(TriggerBoxInfo),
                typeof(BuildingInfo),
            })
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
            this.RefreshSceneDataItems();
        }

        public void LoadSceneData(SceneEditItem sceneEditItem)
        {
            List<SceneEntityManifest> sceneEntityManifests = new List<SceneEntityManifest>();
            string sceneName = sceneEditItem.SceneId.ToString();
            string path = $"{SceneDataDir}{SceneDataItemDirPre}{sceneName}/";
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
            GameObject sceneObject =  this.sceneRoot.transform.Find($"Scene : {sceneEditItem.SceneId.ToString()}").gameObject;
            if (sceneObject!=null)
            {
                Object.Destroy(sceneObject);
            }
        }
        
        public void SaveSceneData(SceneEditItem sceneEditItem)
        {
            
        }

        public void DeleteSceneData(SceneEditItem sceneEditItem)
        {
            
        }

        public List<SceneEditItem> GetSelectSceneEditItems()
        {
            return SceneEditItems.Where(x => x.IsSelect == true).ToList();
        }
        
        

        private void RefreshSceneDataItems()
        {
            this.SceneEditItems.Clear();
            string[] dirs = Directory.GetDirectories(SceneDataDir);
            if ( dirs.Length == 0)
            {
                return;
            }

            foreach (var dir in dirs)
            {
                string subDir = dir.Substring(SceneDataDir.Length);
                Debug.Log(subDir);
                if (subDir.StartsWith(SceneDataItemDirPre))
                {
                    try
                    {
                        int sceneId = int.Parse(subDir.Substring(SceneDataItemDirPre.Length));
                        SceneEditItem sceneEditItem = new SceneEditItem();
                        sceneEditItem.SceneId = sceneId;
                        this.SceneEditItems.Add(sceneEditItem);
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

