using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace ETEditor
{
    public static class CustomEditorDrawHelper
    {
        private static Dictionary<Type, ACustomEditorDrawer> CustomEditorDrawerMap = null;

        public static ACustomEditorDrawer GetCustomEditorDrawer(Type drawType)
        {
            if (CustomEditorDrawerMap==null)
            {
                CustomEditorDrawerMap = new Dictionary<Type, ACustomEditorDrawer>();
                Assembly assembly = Assembly.GetAssembly(typeof(CustomEditorDrawHelper));
                Type[] types = assembly.GetTypes();

                foreach (var type in types)
                {
                    object[] objects = type.GetCustomAttributes(typeof(CustomEditorDrawerAttribute), true);
                    if (objects.Length==0 || type.IsAbstract)
                    {
                        continue;
                    }

                    var attr = objects[0] as CustomEditorDrawerAttribute;
                    if (attr==null)
                    {
                        Debug.LogError("get CustomEditorDrawerAttribute failed!");
                        continue;
                    }

                    ACustomEditorDrawer aDrawer = Activator.CreateInstance(type) as ACustomEditorDrawer;
                    if (aDrawer == null)
                    {
                        Debug.LogError(string.Format("{0} is not ACustomEditorDrawer", type));
                        continue;
                    }
                    CustomEditorDrawerMap.Add(attr.DrawType, aDrawer);
                }
            }

            ACustomEditorDrawer drawer;
            if (CustomEditorDrawerMap.TryGetValue(drawType, out drawer))
            {
                return drawer;
            }
            return null;
        }

        public static void DrawCustomEditor(Rect rect,object value,LiteEntity editorEntity)
        {
            if (value==null)
            {
                return;
            }
            GetCustomEditorDrawer(value.GetType()).OnGui(rect,value, editorEntity);
        }
        
    }
}




