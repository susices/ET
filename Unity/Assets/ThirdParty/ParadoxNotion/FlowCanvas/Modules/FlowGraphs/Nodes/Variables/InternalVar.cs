using System.Linq;
using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [Name("Set Internal Var")]
    [Description("Can be used to set an internal variable, to later be retrieved with a 'Get Internal Var' node. If you do not call 'Set', then the input value will work as a relay instead.")]
    [Category("Variables/Internal")]
    [Color("866693")]
    [ContextDefinedInputs(typeof(Wild))]
    [ExposeAsDefinition]
    abstract public class RelayValueInputBase : FlowNode
    {
        abstract public System.Type relayType { get; }
    }

    ///Relay Input
    public class RelayValueInput<T> : RelayValueInputBase, IEditorMenuCallbackReceiver
    {

        [DelayedField, Tooltip("The identifier name of the internal var")]
        public string identifier = "MyInternalVarName";

        public ValueInput<T> port { get; private set; }
        public bool cached { get; private set; }
        public T cachedValue { get; private set; }

        public override System.Type relayType { get { return typeof(T); } }
        public override string name { get { return string.Format("@ {0}", identifier); } }

        protected override void RegisterPorts() {
            var fOut = AddFlowOutput(" ");
            AddFlowInput("Set", (f) => { cached = true; cachedValue = port.value; fOut.Call(f); });
            port = AddValueInput<T>("Value");
        }

#if UNITY_EDITOR
        void IEditorMenuCallbackReceiver.OnMenu(UnityEditor.GenericMenu menu, Vector2 pos, Port contextPort, object dropInstance) {
            if ( contextPort == null || contextPort.type.IsAssignableFrom(this.relayType) ) {
                menu.AddItem(new GUIContent(string.Format("Variables/Internal/Get '{0}'", identifier)), false, () => { flowGraph.AddFlowNode<RelayValueOutput<T>>(pos, contextPort, dropInstance).SetSource(this); });
            }
        }
#endif

    }


    ///----------------------------------------------------------------------------------------------

    [DoNotList]
    [Description("Returns the selected and previously set Internal Variable's input value.")]
    [Color("866693")]
    [ContextDefinedOutputs(typeof(Wild))]
    abstract public class RelayValueOutputBase : FlowNode
    {
        abstract public void SetSource(RelayValueInputBase source);
    }

    ///Relay Output
    public class RelayValueOutput<T> : RelayValueOutputBase
    {

        [SerializeField]
        private string _sourceInputUID;
        private string sourceInputUID {
            get { return _sourceInputUID; }
            set { _sourceInputUID = value; }
        }

        private System.WeakReference<RelayValueInputBase> _sourceInputRef;
        private RelayValueInput<T> sourceInput {
            get
            {
                RelayValueInputBase reference;
                if ( _sourceInputRef == null ) {
                    reference = graph.GetAllNodesOfType<RelayValueInput<T>>().FirstOrDefault(i => i.UID == sourceInputUID);
                    _sourceInputRef = new System.WeakReference<RelayValueInputBase>(reference);
                }

                _sourceInputRef.TryGetTarget(out reference);
                return reference as RelayValueInput<T>;
            }
        }

        public override string name { get { return string.Format("{0}", sourceInput != null ? sourceInput.ToString() : "@ NONE"); } }

        public override void SetSource(RelayValueInputBase source) {
            _sourceInputUID = source != null ? source.UID : null;
            _sourceInputRef = new System.WeakReference<RelayValueInputBase>(source);
            GatherPorts();
        }

        protected override void RegisterPorts() {
            AddValueOutput<T>("Value", () =>
            {
                if ( sourceInput == null ) { return default(T); }
                return sourceInput.cached ? sourceInput.cachedValue : sourceInput.port.value;
            });
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        protected override void OnNodeExternalGUI() {
            if ( sourceInput != null && ( sourceInput.isSelected || this.isSelected ) ) {
                UnityEditor.Handles.color = Color.grey;
                UnityEditor.Handles.DrawAAPolyLine(rect.center, sourceInput.rect.center);
                UnityEditor.Handles.color = Color.white;
            }
        }

        protected override void OnNodeInspectorGUI() {
            var relayInputs = graph.GetAllNodesOfType<RelayValueInputBase>();
            var newInput = EditorUtils.Popup<RelayValueInputBase>("Internal Var Source", sourceInput, relayInputs);
            if ( newInput != sourceInput ) {
                if ( newInput == null ) {
                    SetSource(null);
                    return;
                }
                if ( newInput.relayType == typeof(T) ) {
                    SetSource(newInput);
                    return;
                }

                var newNode = (RelayValueOutputBase)ReplaceWith(typeof(RelayValueOutput<>).MakeGenericType(newInput.relayType));
                newNode.SetSource((RelayValueInputBase)newInput);
            }
        }

#endif
        ///----------------------------------------------------------------------------------------------

    }
}