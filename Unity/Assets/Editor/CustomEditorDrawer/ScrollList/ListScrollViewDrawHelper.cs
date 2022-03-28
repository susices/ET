using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    
    public static class ListScrollViewDrawHelper
    {
        public static void DrawScrollList(LiteEntity editorEntity)
        {
            if (editorEntity==null)
            {
                //Debug.LogError("customDrawerObject list is null");
                return;
            }

            ScrollListComponent scrollListComponent = editorEntity.GetComponent<ScrollListComponent>();
            if (scrollListComponent==null)
            {
                return;
            }
            
            // ----Titile----

            Rect SaveBtnRect = scrollListComponent.TitleRect;
            SaveBtnRect.width = 30;
            if (GUI.Button(SaveBtnRect, scrollListComponent.SaveIcon))
            {
                scrollListComponent.OnSaveBtnClick?.Invoke(scrollListComponent);
                Debug.Log("Save");
            }
            Rect RefreshBtnIcon = SaveBtnRect;
            RefreshBtnIcon.x += SaveBtnRect.width;
            if (GUI.Button(RefreshBtnIcon, scrollListComponent.RefreshIcon))
            {
                scrollListComponent.OnRefreshBtnClick?.Invoke(scrollListComponent);
                Debug.Log("Refresh");
            }
            
            Rect TitleLabelRect = RefreshBtnIcon;
            TitleLabelRect.x+= RefreshBtnIcon.width+30;
            TitleLabelRect.width = 150;
            EditorGUI.LabelField(TitleLabelRect, scrollListComponent.Title);
            
            Rect AddBtnRect = TitleLabelRect;
            AddBtnRect.x = scrollListComponent.TitleRect.x + scrollListComponent.TitleRect.width - 60;
            AddBtnRect.width = 30;
            if (GUI.Button(AddBtnRect,scrollListComponent.AddIcon))
            {
                scrollListComponent.OnAddItemBtnClick?.Invoke(scrollListComponent);
                if (scrollListComponent.SelectedItemIndex<0)
                {
                    scrollListComponent.SelectedItemIndex = scrollListComponent.list.Count - 1;
                }
                else
                {
                    scrollListComponent.SelectedItemIndex++;
                }
                scrollListComponent.ScrollPosition = new Vector2(0,scrollListComponent.SelectedItemIndex * scrollListComponent.RowHeight);
                GUIUtility.keyboardControl = 0;
            }

            Rect RemoveBtnRect = AddBtnRect;
            RemoveBtnRect.x += AddBtnRect.width;
            if (GUI.Button(RemoveBtnRect,scrollListComponent.RemoveIcon))
            {
                if (scrollListComponent.SelectedItemIndex < 0)
                {
                    Debug.LogWarning("没有选中列表项");
                    return;
                }

                if (scrollListComponent.SelectedItemIndex > scrollListComponent.list.Count-1)
                {
                    Debug.LogWarning("没有选中列表项");
                    scrollListComponent.SelectedItemIndex = -1;
                    return;
                }
                scrollListComponent.list.RemoveAt(scrollListComponent.SelectedItemIndex);
                if (scrollListComponent.SelectedItemIndex == scrollListComponent.list.Count)
                {
                    scrollListComponent.SelectedItemIndex = scrollListComponent.list.Count - 1;
                }
                GUIUtility.keyboardControl = 0;
            }


            // ----ScrollContent----
            var list = scrollListComponent.list;
            if (list==null)
            {
                Debug.LogError("DrawScrollList list is null");
                return;
            }
            
            int rawCount = list.Count;
            Rect totalRect = new Rect(scrollListComponent.ContentRect.x, scrollListComponent.ContentRect.y, scrollListComponent.RowWidth, scrollListComponent.RowHeight * rawCount);
            scrollListComponent.ScrollPosition = GUI.BeginScrollView(scrollListComponent.ContentRect, scrollListComponent.ScrollPosition, totalRect);
            
            int firstRowVisible;
            int lastRowVisible;
            GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible, scrollListComponent.ContentRect.height, rawCount, scrollListComponent.RowHeight,
                ref scrollListComponent.ScrollPosition);

            if (lastRowVisible>=0)
            {
                int numVisibleRows = lastRowVisible - firstRowVisible + 1;
                IterateVisibleItems(scrollListComponent.ContentRect, firstRowVisible, numVisibleRows, scrollListComponent.ContentRect.width, scrollListComponent.ContentRect.height,
                    scrollListComponent.RowHeight, scrollListComponent.ScrollPosition, editorEntity);
            }
            
            GUI.EndScrollView(true);
            Rect totalBorderRect = scrollListComponent.TitleRect;
            totalBorderRect.height += scrollListComponent.ContentRect.height;
            GUI.Box(totalBorderRect, GUIContent.none,scrollListComponent.GreyBorderStyle);
        }

        private static void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible, float viewHeight, int rowCount, int rowHeight,ref Vector2 scrollPos)
        {
            if (rowCount==0)
            {
                firstRowVisible = lastRowVisible = -1;
            }
            else
            {
                float y = scrollPos.y;
                float height = viewHeight;
                firstRowVisible = (int)Mathf.Floor(y / rowHeight);
                lastRowVisible = firstRowVisible + (int)Mathf.Ceil(height / rowHeight);
                firstRowVisible = Mathf.Max(firstRowVisible, 0);
                lastRowVisible = Mathf.Min(lastRowVisible, rowCount - 1);
                if (firstRowVisible >= rowCount && firstRowVisible > 0)
                {
                    scrollPos.y = 0f;
                    GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible, viewHeight, rowCount,rowHeight,ref scrollPos);
                }
            }
        }

        private static void IterateVisibleItems(Rect totalRect ,int firstRow, int numVisibleRows, float rowWidth, float viewHeight, int rowHeight, 
            Vector2 scrollPos, LiteEntity editorEntity)
        {
            
            var scrollListComponent = editorEntity.GetComponent<ScrollListComponent>();
            
            int i = 0;
            while (i<numVisibleRows)
            {
                int drawRow = firstRow + i;
                Rect rowRect = new Rect(totalRect.x, totalRect.y+drawRow * rowHeight, rowWidth, rowHeight);
                float num3 = rowRect.y - scrollPos.y;

                if (num3 <= viewHeight + totalRect.y)
                {
                    var listItem = scrollListComponent.list[drawRow];
                    Rect itemRect = new Rect(rowRect.x + 3f, rowRect.y + 3f, rowRect.width - 6f, rowRect.height - 6f);
                    CustomEditorDrawHelper.DrawCustomEditor(itemRect, listItem, editorEntity);
                    var e = Event.current;
                    switch (e.type)
                    {
                        case EventType.MouseUp:
                            if (!rowRect.Contains(e.mousePosition))
                                break;
                            if (e.button==0)
                            {
                                scrollListComponent.SelectedItemIndex = drawRow;
                                if (scrollListComponent.OnScrollItemLeftClick!=null)
                                {
                                    scrollListComponent.OnScrollItemLeftClick.Invoke(scrollListComponent,drawRow);
                                }
                                e.Use();
                                GUIUtility.keyboardControl = 0;
                            }else if (e.button == 1)
                            {
                                if (scrollListComponent.OnScrollItemRightClick!=null)
                                {
                                    scrollListComponent.OnScrollItemRightClick.Invoke(scrollListComponent,drawRow);
                                }
                                e.Use();
                                GUIUtility.keyboardControl = 0;
                            }
                            break;
                    }
                    if (scrollListComponent.SelectedItemIndex==drawRow && scrollListComponent.ShowSelectBox)
                    {
                        GUI.Box(rowRect, GUIContent.none, scrollListComponent.GreenBorderStyle);
                    }
                    else if(i%2==0)
                    {
                        GUI.Box(rowRect, GUIContent.none, scrollListComponent.GreyBorderStyle);
                    }
                }
                i++;
            }
        }

        public static ScrollListComponent Init(this ScrollListComponent self, IList list, string title, int rowHeight, int rowWidth)
        {
            self.Title = title;
            self.list = list;
            self.RowHeight = rowHeight;
            self.RowWidth = rowWidth;
            self.ShowSelectBox = true;
            self.GreyBorderStyle = new GUIStyle();
            self.GreyBorderStyle.border = new RectOffset(5, 5, 5, 5);
            self.GreyBorderStyle.normal.background = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/CustomEditorDrawer/GrayBorder.png");
            self.GreenBorderStyle = new GUIStyle();
            self.GreenBorderStyle.border = new RectOffset(5, 5, 5, 5);
            self.GreenBorderStyle.normal.background = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/CustomEditorDrawer/GreenBorder.png");
            self.AddIcon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/CustomEditorDrawer/AddIcon.png");
            self.RemoveIcon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/CustomEditorDrawer/RemoveIcon.png");
            self.SaveIcon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/CustomEditorDrawer/SaveIcon.png");
            self.RefreshIcon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/CustomEditorDrawer/RefreshIcon.png");
            return self;
        }
    }
}

