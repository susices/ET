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
            Type[] types = ReflectionTools.GetImplementationsOf(typeof (ISceneEntityInfo));
            foreach (Type type in types)
            {
                SceneEntityManifest manifest = new SceneEntityManifest();
                manifest.SceneId = this.SceneId;
                SceneEditorHelper.SaveSceneEntityManifestFile(manifest,type);
            }
        }
    }
}

