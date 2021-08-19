using System;
using System.Reflection;
using Slate;
using UnityEditor;
using UnityEngine;

namespace ET
{
    [CustomEditor(typeof(Cutscene))]
    public class CutSceneEditor:Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // Cutscene cutscene = (Cutscene)target;
            // FieldInfo totalHeightFieldInfo = cutscene.GetType().GetField("totalHeight",
            //     BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            // var totalHeight = (float)totalHeightFieldInfo.GetValue(cutscene);
            //
            // FieldInfo TOP_MARGINFieldInfo = cutscene.GetType().GetField("TOP_MARGIN",
            //     BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            // var TOP_MARGIN = (float)TOP_MARGINFieldInfo.GetValue(cutscene);
            //
            // FieldInfo TOOLBAR_HEIGHTFieldInfo = cutscene.GetType().GetField("TOOLBAR_HEIGHT",
            //     BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            // var TOOLBAR_HEIGHT = (float)TOOLBAR_HEIGHTFieldInfo.GetValue(cutscene);
            //
            // FieldInfo leftRectFieldInfo = cutscene.GetType().GetField("leftRect",
            //     BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            // var leftRect = (Rect)leftRectFieldInfo.GetValue(cutscene);
            //
            // var addButtonY = totalHeight + TOP_MARGIN + TOOLBAR_HEIGHT + 50;
            // var addRect = Rect.MinMaxRect(leftRect.xMin + 10, addButtonY, leftRect.xMax - 10, addButtonY + 20);
            // GUI.color = Color.white.WithAlpha(0.5f);
            // if ( GUI.Button(addRect, "Add DynamicActor Group") ) {
            //     var newGroup = cutscene.AddGroup<ActorGroup>(null).AddTrack<ActorActionTrack>();
            //     CutsceneUtility.selectedObject = newGroup;
            // }
            
            Debug.Log("gui");
        }
    }
}