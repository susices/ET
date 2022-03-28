using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETEditor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomEditorDrawerAttribute : Attribute
    {
        public Type DrawType;
        public CustomEditorDrawerAttribute(Type type)
        {
            DrawType = type;
        }
    }
}

