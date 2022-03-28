using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
	public class ScrollListExample : EditorWindow
    {
        [MenuItem("Tools/CustomEditorDrawer/ScrollList")]
        public static void Open()
        {
            var window = GetWindow<ScrollListExample>();
            window.Show();
        }

        private List<ExampleBaseItem> itemList;

        public LiteEntity scrollEntity;

        private void OnEnable()
        {
            if (scrollEntity==null)
            {
                scrollEntity = LiteEntity.Create<LiteEntity>();
                itemList = new List<ExampleBaseItem>();
                for (int i = 0; i < 20; i++)
                {
                    itemList.Add(new ExampleItemA());
                    itemList.Add(new ExampleItemB());
                }
                
                var scrollListComponent = scrollEntity.AddComponent<ScrollListComponent>().Init(this.itemList, "测试列表", 30,150);
                
                scrollListComponent.OnScrollItemRightClick += this.OnScrollListRightClick;
                scrollListComponent.OnAddItemBtnClick += this.OnAddBtnClick;
            }
        }

        private void OnGUI()
        {
            Rect titleRect = EditorGUILayout.GetControlRect(false, 30);
            Rect contentRect = EditorGUILayout.GetControlRect(false, 400);
            var scrollListcomponent = this.scrollEntity.GetComponent<ScrollListComponent>();
            scrollListcomponent.TitleRect = titleRect;
            scrollListcomponent.ContentRect = contentRect;
            ListScrollViewDrawHelper.DrawScrollList(this.scrollEntity);
        }

        private void OnScrollListRightClick(ScrollListComponent scrollListComponent,int index)
        {
            var list = scrollListComponent.list;
            if (list==null)
            {
                return;
            }
            var genericMenu = new GenericMenu();
            // 弹出枚举选择框 从当前位置 添加list元素
            genericMenu.AddItem(new GUIContent("添加"), false, () =>
            {   
                
            });
            genericMenu.AddItem(new GUIContent("删除"), false, () =>
            {   
                list.RemoveAt(index);
            });
            // 将当前元素缓存 从list移除
            genericMenu.AddItem(new GUIContent("剪切"), false, () =>
            {   
                
            });
            // 将缓存元素插入至当前位置  删除缓存
            genericMenu.AddItem(new GUIContent("粘贴"), false, () =>
            {   
                
            });
            
            genericMenu.AddItem(new GUIContent("功能合集/功能2"), false, () => { Debug.Log("功能2"); });
            genericMenu.AddItem(new GUIContent("功能合集/功能3"), false, () => { Debug.Log("功能3"); });
            genericMenu.AddSeparator("功能合集/");
            genericMenu.AddItem(new GUIContent("功能合集/功能4"), false, () => { Debug.Log("功能4"); });
            
            genericMenu.ShowAsContext();
        }

        private void OnAddBtnClick(ScrollListComponent scrollListComponent)
        {
            if(scrollListComponent.SelectedItemIndex>=0)
            {
                scrollListComponent.list.Insert(scrollListComponent.SelectedItemIndex+1, new ExampleItemA()
                {
                    A = "111",
                    B = 333,
                    C = 4.44f,
                });
            }
            else
            {
                scrollListComponent.list.Insert(scrollListComponent.list.Count, new ExampleItemA()
                {
                    A = "111",
                    B = 333,
                    C = 4.44f,
                });
            }

        }
    }
}

