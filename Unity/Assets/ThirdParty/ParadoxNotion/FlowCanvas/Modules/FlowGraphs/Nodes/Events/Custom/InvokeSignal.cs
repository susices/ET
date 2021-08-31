using UnityEngine;
using ParadoxNotion.Design;
using NodeCanvas.Framework;

namespace FlowCanvas.Nodes
{

    [Category("Events/Signals")]
    [Description("Invoke a defined Signal. Signals are similar to name-based events but are safer to changes and support multiple parameters.")]
    [HasRefreshButton, DropReferenceType(typeof(SignalDefinition))]
    public class InvokeSignal : FlowNode, IDropedReferenceNode
    {
        [GatherPortsCallback]
        public BBParameter<SignalDefinition> _signalDefinition;
        [GatherPortsCallback]
        public bool global;

        private SignalDefinition signalDefinition {
            get { return _signalDefinition.value; }
            set { _signalDefinition.value = value; }
        }

        private ValueInput<Transform> target;
        private ValueInput[] inputArgs;

        public override string name {
            get { return base.name + string.Format(" [ <color=#DDDDDD>{0}</color> ]", signalDefinition != null ? signalDefinition.name : "NULL"); }
        }

        public void SetTarget(Object target) {
            signalDefinition = (SignalDefinition)target;
            GatherPorts();
        }

        protected override void RegisterPorts() {
            if ( !global ) {
                target = AddValueInput<Transform>("Target");
            }

            if ( signalDefinition != null ) {
                inputArgs = new ValueInput[signalDefinition.parameters.Count];
                for ( var i = 0; i < signalDefinition.parameters.Count; i++ ) {
                    var parameter = signalDefinition.parameters[i];
                    inputArgs[i] = AddValueInput(parameter.name, parameter.type, parameter.ID);
                }
            }

            var exit = AddFlowOutput(" ");
            AddFlowInput(" ", (f) =>
            {
                var args = new object[signalDefinition.parameters.Count];
                for ( var i = 0; i < signalDefinition.parameters.Count; i++ ) {
                    args[i] = inputArgs[i].value;
                }
                signalDefinition.Invoke(graphAgent.transform, global ? null : target.value, global, args);
                f.Call(exit);
            });
        }
    }
}