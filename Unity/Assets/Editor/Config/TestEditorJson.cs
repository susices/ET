using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ET.ConfigEditor
{
    public static class TestEditorJson 
    {
        [MenuItem("Tools/SaveJson")]
        public static void TestSave()
        {
            var aiConfig = new AIConfig();
            aiConfig.Desc = "Desc";
            aiConfig.Id = 101;
            aiConfig.Name = "AITest";
            aiConfig.AIConfigId = 1;
            aiConfig.SaveJsonFile(Application.dataPath+"/a.json");
        }
        
        [MenuItem("Tools/LoadJson")]
        public static void TestLoad()
        {
            var aiConfig = new AIConfig();
            aiConfig.LoadJsonFile(Application.dataPath+"/a.json");
            var b = aiConfig;
        }
    }
}

