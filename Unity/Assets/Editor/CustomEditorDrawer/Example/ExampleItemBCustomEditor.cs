using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    [CustomEditorDrawer(typeof(ExampleItemB))]
    public class ExampleItemBCustomEditor : ACustomEditorDrawer
    {
        public override void OnGui(Rect rect, object objValue, LiteEntity editorEntity)
        {
            var obj = objValue as ExampleItemB;
            if (obj==null)
            {
                Debug.LogError("ExampleItemBCustomEditor can not get value");
                return;
            }
            rect.width = 50;
            EditorGUI.LabelField(rect, "D");
            rect.x += rect.width;
            obj.D = EditorGUI.TextField(rect, obj.D);
        }
    }
}

