using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETEditor
{
    [Serializable]
    public class SceneEditItem
    {
        [ToggleLeft]
        [LabelText("")]
        public bool IsSelect;
        
        public int SceneId = 0;
    }
}

