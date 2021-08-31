using System.Linq;
using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{
    [DoNotList]
    [Name("Function Call")]
    [Description("Calls an existing Custom Function")]
    [Category("Functions/Custom")]
    [ParadoxNotion.Serialization.DeserializeFrom("FlowCanvas.Nodes.RelayFlowInput")]
    [HasRefreshButton]
    public class CustomFunctionCall : FlowControlNode
    {

        [SerializeField]
        private string _sourceOutputUID;
        private string sourceFunctionUID {
            get { return _sourceOutputUID; }
            set { _sourceOutputUID = value; }
        }

        private ValueInput[] portArgs;
        private object[] objectArgs;
        private FlowOutput fOut;

        private System.WeakReference<CustomFunctionEvent> _sourceFunctionRef;
        public CustomFunctionEvent sourceFunction {
            get
            {
                CustomFunctionEvent reference;
                if ( _sourceFunctionRef == null ) {
                    reference = graph.GetAllNodesOfType<CustomFunctionEvent>().FirstOrDefault(i => i.UID == sourceFunctionUID);
                    _sourceFunctionRef = new System.WeakReference<CustomFunctionEvent>(reference);
                }

                _sourceFunctionRef.TryGetTarget(out reference);
                return reference;
            }
        }

        public override string name {
            get { return string.Format("Call {0} ()", sourceFunction != null ? sourceFunction.identifier : "NONE"); }
        }

        public override string description {
            get { return sourceFunction != null && !string.IsNullOrEmpty(sourceFunction.comments) ? sourceFunction.comments : base.description; }
        }

        //...
        public void SetFunction(CustomFunctionEvent func) {
            _sourceOutputUID = func != null ? func.UID : null;
            _sourceFunctionRef = new System.WeakReference<CustomFunctionEvent>(func);
            GatherPorts();
        }

        //...
        protected override void RegisterPorts() {
            AddFlowInput(SPACE, Invoke);
            if ( sourceFunction != null ) {
                var parameters = sourceFunction.parameters;
                portArgs = new ValueInput[parameters.Count];
                for ( var _i = 0; _i < parameters.Count; _i++ ) {
                    var i = _i;
                    var parameter = parameters[i];
                    portArgs[i] = AddValueInput(parameter.name, parameter.type, parameter.ID);
                }

                if ( sourceFunction.returns.type != null ) {
                    AddValueOutput(sourceFunction.returns.name, sourceFunction.returns.ID, sourceFunction.returns.type, sourceFunction.GetReturnValue);
                }

                fOut = AddFlowOutput(SPACE);
            }
        }

        //...
        void Invoke(Flow f) {
            if ( sourceFunction != null ) {
                if ( objectArgs == null ) {
                    objectArgs = new object[portArgs.Length];
                }
                for ( var i = 0; i < portArgs.Length; i++ ) {
                    objectArgs[i] = portArgs[i].value;
                }
                sourceFunction.InvokeAsync(f, fOut.Call, objectArgs);
            }
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        protected override void OnNodeExternalGUI() {
            if ( sourceFunction != null && isSelected ) {
                UnityEditor.Handles.color = Color.grey;
                UnityEditor.Handles.DrawAAPolyLine(rect.center, sourceFunction.rect.center);
                UnityEditor.Handles.color = Color.white;
            }
        }
        protected override void OnNodeInspectorGUI() {
            if ( sourceFunction == null ) {
                var functions = graph.GetAllNodesOfType<CustomFunctionEvent>();
                var currentFunc = functions.FirstOrDefault(i => i.UID == sourceFunctionUID);
                var newFunc = EditorUtils.Popup<CustomFunctionEvent>("Target Function", currentFunc, functions);
                if ( newFunc != currentFunc ) {
                    SetFunction(newFunc);
                }
            }
            base.OnNodeInspectorGUI();
        }

#endif
        ///----------------------------------------------------------------------------------------------
    }
}