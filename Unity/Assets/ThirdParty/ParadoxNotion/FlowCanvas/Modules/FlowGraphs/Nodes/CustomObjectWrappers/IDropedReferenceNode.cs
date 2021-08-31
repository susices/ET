namespace FlowCanvas.Nodes
{
    ///Implement in a node to have it work with drag and drop
    ///The [DropReferenceType] attribute is also required on the node
    public interface IDropedReferenceNode : NodeCanvas.Framework.IGraphElement
    {
        void SetTarget(UnityEngine.Object target);
    }
}