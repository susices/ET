using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace ETEditor
{
    [CustomEditorDrawer(typeof(ExampleItemA))]
    public class ExampleItemACustomEditor: ACustomEditorDrawer
    {
        public override void OnGui(Rect rect,object objValue, LiteEntity editorEntity)
        {
            var obj = objValue as ExampleItemA;
            if (obj==null)
            {
                Debug.LogError("ExampleItemACustomEditor can not get value");
                return;
            }
        
            var drawRect = rect;
            drawRect.width = 50;
            EditorGUI.LabelField(rect, "A");
            drawRect.x += drawRect.width;
        
            obj.A = EditorGUI.TextField(drawRect, obj.A);
            drawRect.x += drawRect.width;
            EditorGUI.LabelField(drawRect, "B");
            drawRect.x += drawRect.width;
            obj.B = EditorGUI.IntField(drawRect, obj.B);
            drawRect.x += drawRect.width;
            EditorGUI.LabelField(drawRect,"C");
            drawRect.x += drawRect.width;
            drawRect.width = 150;
            obj.C = EditorGUI.Slider(drawRect, obj.C, 0f, 10f);
            drawRect.x += rect.width;
            drawRect.width = 50;
        }
    }
}

