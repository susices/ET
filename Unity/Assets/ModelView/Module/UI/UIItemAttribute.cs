using System;

namespace ET
{
    public class UIItemAttribute : Attribute
    {
        public int UIItemType { get; }

        public UIItemAttribute(int uiItemType)
        {
            this.UIItemType = uiItemType;
        }
    }
}