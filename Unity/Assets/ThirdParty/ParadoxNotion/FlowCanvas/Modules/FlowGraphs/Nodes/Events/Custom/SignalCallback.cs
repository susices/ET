using UnityEngine;
using ParadoxNotion.Design;
using NodeCanvas.Framework;

namespace FlowCanvas.Nodes
{

    [Category("Events/Signals")]
    [Description("Check if a defined Signal has been invoked. Signals are similar to name-based events but are safer to changes and support multiple parameters.")]
    [HasRefreshButton, DropReferenceType(typeof(SignalDefinition))]
    [ExecutionPriority(100)]
    public class SignalCallback : RouterEventNode<Transform>, IDropedReferenceNode
    {
        [GatherPortsCallback]
        public BBParameter<SignalDefinition> _signalDefinition;
        private SignalDefinition signalDefinition {
            get { return _signalDefinition.value; }
            set { _signalDefinition.value = value; }
        }

        private FlowOutput onReceived;
        private Transform receiver;
        private Transform sender;

        private object[] args;

        public override string name {
            get { return base.name + string.Format(" [ <color=#DDDDDD>{0}</color> ]", signalDefinition != null ? signalDefinition.name : "NULL"); }
        }

        public void SetTarget(Object target) {
            signalDefinition = (SignalDefinition)target;
            GatherPorts();
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) { }
        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) { }

        public override void OnPostGraphStarted() {
            base.OnPostGraphStarted();
            if ( signalDefinition != null ) {
                signalDefinition.onInvoke -= OnInvoked;
                signalDefinition.onInvoke += OnInvoked;
            }
        }

        public override void OnGraphStoped() {
            base.OnGraphStoped();
            if ( signalDefinition != null ) {
                signalDefinition.onInvoke -= OnInvoked;
            }
        }

        protected override void RegisterPorts() {
            onReceived = AddFlowOutput("Received");
            AddValueOutput<Transform>("Receiver", () => receiver);
            AddValueOutput<Transform>("Sender", () => sender);

            if ( signalDefinition != null ) {
                for ( var _i = 0; _i < signalDefinition.parameters.Count; _i++ ) {
                    var i = _i;
                    var parameter = signalDefinition.parameters[i];
                    AddValueOutput(parameter.name, parameter.ID, parameter.type, () => args[i]);
                }
            }
        }

        void OnInvoked(Transform sender, Transform receiver, bool isGlobal, object[] args) {
            this.receiver = ResolveReceiver(receiver != null ? receiver.gameObject : null);
            if ( this.receiver != null || isGlobal ) {
                this.sender = sender;
                this.args = args;
                onReceived.Call(new Flow());
            }
        }
    }
}
