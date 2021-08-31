using System.Linq;
using UnityEngine;
using ParadoxNotion.Design;
using NodeCanvas.Framework;

namespace FlowCanvas.Nodes
{

    [Name("GOTO Label Definition", 1)]
    [Description("A Flow Control Label definition. Can be called with the GOTO node.")]
    [Category("Flow Controllers/GOTO")]
    [Color("ff5c5c")]
    [ContextDefinedOutputs(typeof(Flow))]
    public class GoToLabel : FlowControlNode, IEditorMenuCallbackReceiver
    {

        [DelayedField, Tooltip("The identifier name of the label")]
        public string identifier = "MY_LABEL";
        [HideInInspector]
        public FlowOutput port { get; private set; }

        public override string name {
            get { return string.Format("[ {0} ]", identifier.ToUpper()); }
        }

        protected override void RegisterPorts() {
            port = AddFlowOutput(" ");
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        void IEditorMenuCallbackReceiver.OnMenu(UnityEditor.GenericMenu menu, Vector2 pos, Port contextPort, object dropInstance) {
            if ( contextPort == null || contextPort is FlowOutput ) {
                menu.AddItem(new GUIContent(string.Format("Flow Controllers/GOTO/GOTO '{0}'", identifier)), false, () => { flowGraph.AddFlowNode<GoToStatement>(pos, contextPort, dropInstance).SetTarget(this); });
            }
        }
#endif
        ///----------------------------------------------------------------------------------------------

    }


    ///----------------------------------------------------------------------------------------------

    [DoNotList]
    [Description("Routes the Flow to the target GOTO label.")]
    [ContextDefinedInputs(typeof(Flow))]
    public class GoToStatement : FlowControlNode, IHaveNodeReference
    {
        [ParadoxNotion.Serialization.FullSerializer.fsSerializeAs("_targetLabelUID")]
        public NodeReference<GoToLabel> _targetLabel;

        INodeReference IHaveNodeReference.targetReference => _targetLabel;
        private GoToLabel target => _targetLabel?.Get(graph);

        public override string name => string.Format("GOTO {0}", target != null ? target.ToString() : "NONE");

        public void SetTarget(GoToLabel newTarget) { _targetLabel = new NodeReference<GoToLabel>(newTarget); }

        protected override void RegisterPorts() {
            AddFlowInput(" ", (f) => { if ( target != null ) target.port.Call(f); });
        }
    }
}