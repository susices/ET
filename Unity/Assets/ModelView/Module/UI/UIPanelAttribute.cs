using System;

namespace ET
{
    public class UIPanelAttribute : BaseAttribute
    {
        public int UIPanelType { get; }

        public UIPanelAttribute(int uiPanelType)
        {
            this.UIPanelType = uiPanelType;
        }
    }
}