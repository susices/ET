using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    [Name("Draw Gizmos")]
    [Category("Events/Object")]
    [Description("Calls Gizmos")]
    public class DrawGizmosEvents : RouterEventNode<Transform>
    {

        private FlowOutput onDrawGizmos;
        private GameObject receiver;

        protected override void RegisterPorts() {
            onDrawGizmos = AddFlowOutput("On Draw Gizmos");
            AddValueOutput<GameObject>("Receiver", () => { return receiver; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onDrawGizmos += OnDrawGizmos;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onDrawGizmos -= OnDrawGizmos;
        }

        void OnDrawGizmos(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver).gameObject;
            onDrawGizmos.Call(new Flow());
        }
    }
}