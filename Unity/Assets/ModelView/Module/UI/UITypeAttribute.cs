using System;

namespace ET
{
    public class UITypeAttribute : BaseAttribute
    {
        public int UIType { get; }

        public UITypeAttribute(int UIType)
        {
            this.UIType = UIType;
        }
    }
}