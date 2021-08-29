using System;
using System.Collections;
using System.Collections.Generic;
using ETEditor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class SceneMenuTreeWindow : OdinMenuEditorWindow
{
    [MenuItem("Tools/场景编辑器窗口")]
    private static void OpenWindow()
    {
        var window = GetWindow<SceneMenuTreeWindow>();
        window.MenuWidth = 150;
        window.minSize = new Vector2(600, 200);
        window.Show();
    }
    
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        // tree.Add("场景管理",CreateInstance<SceneEditorWindow>());
        OdinMenuItem sceneCreateEditMenuItem = new OdinMenuItem(tree, "创建场景",  CreateInstance<SceneCreateEditorWindow>());
        tree.AddMenuItemAtPath("场景管理", sceneCreateEditMenuItem);
        OdinMenuItem sceneEditMenuItem = new OdinMenuItem(tree, "加载场景",  CreateInstance<SceneEditorWindow>());
        tree.AddMenuItemAtPath("场景管理", sceneEditMenuItem);

        tree.Selection.SelectionChanged += this.OnTreeSelection;
        return tree;
    }

    private void OnTreeSelection(SelectionChangedType selectionChangedType)
    {
        switch (this.MenuTree.Selection.SelectedValue)
        {
            case SceneEditorWindow sceneEditorWindow:
                sceneEditorWindow.RefreshSceneDataItems();
                break;
        }
    }
}
