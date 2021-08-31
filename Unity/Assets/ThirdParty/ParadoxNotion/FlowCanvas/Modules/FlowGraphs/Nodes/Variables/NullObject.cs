using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    [Name("Null")]
    [Category("Variables")]
    [Description("Simply returns a NULL")]
    [ContextDefinedOutputs(typeof(Wild))]
    public class NullObject : FlowNode
    {
        protected override void RegisterPorts() {
            AddValueOutput<object>("Value", () => { return null; });
        }
    }
}