using System;

namespace ET
{
    public class UIPanelComponentAttribute : BaseAttribute
    {
        public int ComponentIndex { get; }

        public UIPanelComponentAttribute(int componentIndex)
        {
            this.ComponentIndex = componentIndex;
        }
    }
}