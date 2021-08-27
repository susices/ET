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
        OdinMenuItem sceneEditMenuItem = new OdinMenuItem(tree, "场景管理",  CreateInstance<SceneEditorWindow>());
        tree.AddMenuItemAtPath(null, sceneEditMenuItem);
        return tree;
    }
}
