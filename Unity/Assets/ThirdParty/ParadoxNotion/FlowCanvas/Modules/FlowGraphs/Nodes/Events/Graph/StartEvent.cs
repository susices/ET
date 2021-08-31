using System.Collections;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [Name("On Start", 8)]
    [Category("Events/Graph")]
    [Description("Called only once and the first time the Graph is enabled.\nThis is called after all OnAwake events of all graphs and after OnEnable events of this graph.")]
    [ExecutionPriority(8)]
    public class StartEvent : EventNode
    {

        private FlowOutput start;
        private bool called;

        public override void OnPostGraphStarted() {
            //we only care about subscribe. the event is nulled after call
            if ( graphAgent is NodeCanvas.Framework.GraphOwner ) {
                var owner = ( graphAgent as NodeCanvas.Framework.GraphOwner );
                if ( !owner.startCalled ) {
                    owner.onMonoBehaviourStart += OnStartCallback;
                    return;
                }
            }

            OnStartCallback();
        }

        void OnStartCallback() {
            if ( !called && flowGraph.isRunning ) {
                called = true;
                start.Call(new Flow());
            }
        }

        protected override void RegisterPorts() {
            start = AddFlowOutput("Once");
        }
    }
}