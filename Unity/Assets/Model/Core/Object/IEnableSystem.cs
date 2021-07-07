using System;

namespace ET
{
    public interface IEnableSystem
    {
        Type Type();
        ETTask Run(object o);
    }
    
    [ObjectSystem]
    public abstract class EnableSystem<T> : IEnableSystem
    {
        public async ETTask Run(object o)
        {
            await this.Enable((T)o);
        }

        public Type Type()
        {
            return typeof(T);
        }

        public abstract ETTask Enable(T self);
    }
}