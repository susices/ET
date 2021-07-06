using System;

namespace ET
{
    public interface IDisableSystem
    {
        Type Type();
        void Run(object o);
    }
    
    [ObjectSystem]
    public abstract class DisableSystem<T> : IDisableSystem
    {
        public void Run(object o)
        {
            this.Disable((T)o);
        }

        public Type Type()
        {
            return typeof(T);
        }

        public abstract void Disable(T self);
    }
}