using ET.ConfigEditor;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    [CustomEditorDrawer(typeof(NumericInfo))]
    public class NumericInfoCustomEditor : ACustomEditorDrawer
    {
        public override void OnGui(Rect rect, object value, LiteEntity editorEntity)
        {
            var obj = value as NumericInfo;
            if (value==null)
            {
                Debug.Log("value==null");
                return;
            }
            var drawRect = rect;
            drawRect.width = 60;
            EditorGUI.LabelField(drawRect, "Name");
            drawRect.x += drawRect.width;
            drawRect.width = 100;
            EditorGUI.BeginChangeCheck();
            obj.Name = EditorGUI.TextField(drawRect, obj.Name);
            EditorGUI.EndChangeCheck();
            drawRect.x += drawRect.width;
            drawRect.width = 60;
            EditorGUI.LabelField(drawRect, "Base");
            drawRect.x += drawRect.width;
            drawRect.width = 30;
            obj.Base = EditorGUI.Toggle(drawRect, obj.Base);
            drawRect.x += drawRect.width;
            drawRect.width = 60;
            EditorGUI.LabelField(drawRect, "Add");
            drawRect.x += drawRect.width;
            drawRect.width = 30;
            obj.Add = EditorGUI.Toggle(drawRect, obj.Add);
            drawRect.x += drawRect.width;
            drawRect.width = 60;
            EditorGUI.LabelField(drawRect, "Pct");
            drawRect.x += drawRect.width;
            drawRect.width = 30;
            obj.Pct = EditorGUI.Toggle(drawRect, obj.Pct);
            drawRect.x += drawRect.width;
            drawRect.width = 60;
            EditorGUI.LabelField(drawRect, "FinalAdd");
            drawRect.x += drawRect.width;
            drawRect.width = 30;
            obj.FinalAdd = EditorGUI.Toggle(drawRect, obj.FinalAdd);
            drawRect.x += drawRect.width;
            drawRect.width = 60;
            EditorGUI.LabelField(drawRect, "FinalPct");
            drawRect.x += drawRect.width;
            drawRect.width = 30;
            obj.FinalPct = EditorGUI.Toggle(drawRect, obj.FinalPct);
        }
    }
}