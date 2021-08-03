using System;

namespace ET
{
    /// <summary>
    /// Buff状态
    /// </summary>
    [Flags]
    public enum BuffState
    {
        None = 0,
        Ice,
        Fire,
        Water,
        Thunder,
    }
}