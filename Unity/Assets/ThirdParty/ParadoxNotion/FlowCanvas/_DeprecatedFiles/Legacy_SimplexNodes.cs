using System;
using System.Collections.Generic;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [Obsolete]
    public class SwitchValue<T> : PureFunctionNode<T, bool, T, T>
    {
        public override T Invoke(bool condition, T isTrue, T isFalse) {
            return condition ? isTrue : isFalse;
        }
    }

    [Obsolete]
    public class PickValue<T> : PureFunctionNode<T, int, IList<T>>
    {
        public override T Invoke(int index, IList<T> values) {
            try { return values[index]; }
            catch { return default(T); }
        }
    }
}