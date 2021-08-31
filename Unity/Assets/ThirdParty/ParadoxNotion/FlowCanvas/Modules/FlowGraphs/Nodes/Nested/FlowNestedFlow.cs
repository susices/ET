using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{
    [Name("Sub Flow")]
    [DropReferenceType(typeof(FlowCanvas.FlowScript))]
    public class FlowNestedFlow : FlowNestedBase<FlowCanvas.FlowScript>
    {

    }
}