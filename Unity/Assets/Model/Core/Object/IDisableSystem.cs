using System;

namespace ET
{
    public interface IDisableSystem
    {
        Type Type();
        ETTask Run(object o);
    }
    
    [ObjectSystem]
    public abstract class DisableSystem<T> : IDisableSystem
    {
        public async ETTask Run(object o)
        {
            await this.Disable((T)o);
        }

        public Type Type()
        {
            return typeof(T);
        }

        public abstract ETTask Disable(T self);
    }
}