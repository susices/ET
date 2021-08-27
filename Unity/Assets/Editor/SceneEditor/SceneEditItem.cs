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
        [TableColumnWidth(70,false)]
        public bool IsSelect;
        
        [ReadOnly]
        public int SceneId = 0;

        [ReadOnly]
        public bool IsLoaded;

    }
}

