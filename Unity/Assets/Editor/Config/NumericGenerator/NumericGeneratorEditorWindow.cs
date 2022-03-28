using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ETEditor;
using Scriban;
using UnityEditor;
using UnityEngine;

namespace ET.ConfigEditor
{
    public class NumericGeneratorEditorWindow: EditorWindow
    {
        private LiteEntity numericInfoEntity;
        
        private NumericInfoList numericInfoList;
        private string dataPath
        {
            get
            {
                return Application.dataPath + "/../../ConfigInput/ET/NumericInfoList.json";
            }
        }
        
        [MenuItem("Tools/Config/NumericGenerator")]
        public static void Open()
        {
            var window = GetWindow<NumericGeneratorEditorWindow>();
            window.minSize = new Vector2(600, 800);
            window.Init();
            window.Show();
        }

        private void OnGUI()
        {
            if (numericInfoEntity==null)
            {
                this.Init();
                
            }
            Rect titleRect = EditorGUILayout.GetControlRect(false, 30);
            Rect contentRect = EditorGUILayout.GetControlRect(false, 400);
            var scrollListcomponent = this.numericInfoEntity.GetComponent<ScrollListComponent>();
            scrollListcomponent.TitleRect = titleRect;
            scrollListcomponent.ContentRect = contentRect;
            ListScrollViewDrawHelper.DrawScrollList(this.numericInfoEntity);
            Rect GenerateBtnRect = EditorGUILayout.GetControlRect(false, 30);
            if (GUI.Button(GenerateBtnRect, "代码生成"))
            {
                this.CodeGenerate();
            }
        }
        
        private void Init()
        { 
            numericInfoEntity = LiteEntity.Create<LiteEntity>();
            LoadData();
            this.numericInfoEntity.AddComponent<ScrollListComponent>().Init(this.numericInfoList.List, "test", 30, 200);
            var scrollList = this.numericInfoEntity.GetComponent<ScrollListComponent>();
            scrollList.OnSaveBtnClick += this.SaveData;
            scrollList.OnRefreshBtnClick += this.Refresh;
            scrollList.OnAddItemBtnClick += this.OnAdd;
        }
        
        private NumericInfoList LoadData()
        {
            numericInfoList = new NumericInfoList();
            if (File.Exists(this.dataPath))
            {
                numericInfoList.LoadJsonFile(this.dataPath);
            }
            else
            {
                numericInfoList.SaveJsonFile(this.dataPath);
            }
        
            return numericInfoList;
        }

        private void OnAdd(ScrollListComponent scrollListComponent)
        {
            if (scrollListComponent.SelectedItemIndex<0)
            {
                scrollListComponent.SelectedItemIndex = scrollListComponent.list.Count;
            }

            if (scrollListComponent.list.Count==0)
            {
                scrollListComponent.list.Insert(0, new NumericInfo());
            }
            else
            {
                scrollListComponent.list.Insert(scrollListComponent.SelectedItemIndex+1, new NumericInfo());
            }
        }

        private void Refresh(ScrollListComponent scrollListComponent)
        {
            scrollListComponent.list = LoadData().List;
        }
        
        private void SaveData(ScrollListComponent scrollListComponent)
        {
            this.numericInfoList.SaveJsonFile(this.dataPath);
        }

        private void CodeGenerate()
        {
            var text = File.ReadAllText("Assets/Editor/Config/NumericGenerator/NumericGenerator.tpl");
            Template template = Template.Parse(text);
            string result = template.Render( numericInfoList);
            File.WriteAllText(Application.dataPath + "/../../Unity/Codes/Model/Module/Numeric/NumericType.cs", result);
            Debug.Log("CodeGenerate");
        }
    }
}

