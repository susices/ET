using NodeCanvas.Framework;

namespace FlowCanvas.Nodes
{
    abstract public class ParameterVariableNode : FlowNode
    {
        abstract public BBParameter parameter { get; }
    }
}