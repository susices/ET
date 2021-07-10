using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class MyAssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        // //监听"资源即将被创建"事件
        // public static void OnWillCreateAsset(string path)
        // {
        //     Debug.LogFormat("Create AssetPath:{0}", path);
        // }
        //
        // //监听"资源即将被移动"事件
        // public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
        // {
        //     Debug.LogFormat("Move from:{0} to:{1}", oldPath, newPath);
        //     return AssetMoveResult.DidNotMove;
        // }
    }
}

