using System;

namespace ET
{
    public class UIPanelAttribute : BaseAttribute
    {
        public int UIType { get; }

        public UIPanelAttribute(int UIType)
        {
            this.UIType = UIType;
        }
    }
}