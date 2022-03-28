
using UnityEngine;

namespace ETEditor
{
    public abstract class ACustomEditorDrawer
    {
        public abstract void OnGui(Rect rect,object value, LiteEntity editorEntity);
    }
}

