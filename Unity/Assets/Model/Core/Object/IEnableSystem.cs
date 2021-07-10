using System;

namespace ET
{
    public interface IEnableSystem
    {
        Type Type();
    }

    public interface IEnable
    {
        ETTask Run(object o);
    }

    public interface IEnable<T>
    {
        ETTask Run(object o, T args);
    }
    
    
    [ObjectSystem]
    public abstract class EnableSystem<T> : IEnableSystem, IEnable
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
    
    [ObjectSystem]
    public abstract class EnableSystem<T,A> : IEnableSystem, IEnable<A>
    {
        public Type Type()
        {
            return typeof(T);
        }

        public abstract ETTask Enable(T self, A args);
        public async ETTask Run(object o, A args)
        {
            await this.Enable((T)o, args);
        }
    }
}