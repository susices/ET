namespace FlowCanvas.Nodes
{
    ///Implement in a UnityObject to make it possible to add as node in a flowScript
    public interface IExternalImplementedNode
    {
        void RegisterPorts(FlowNode parent);
    }
}