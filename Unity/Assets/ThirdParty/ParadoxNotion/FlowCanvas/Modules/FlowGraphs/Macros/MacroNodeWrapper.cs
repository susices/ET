using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Macros
{

    [DoNotList]
    [Color("ffe4e1")]
    [HasRefreshButton]
    [DropReferenceType(typeof(Macro))]
    public class MacroNodeWrapper : FlowNode, IGraphAssignable, IUpdatable
    {
        //Remarks: Usually IGraphAssignables work differently and instances are created in extension method CheckInstance.
        //In this case however we dont need multiple instances since in runtime macro ref can't change like it does in other subgraph systems.
        //We still need to store the instance and return it in a dictionary for cleanup though, even if that dict contains one element.

        [SerializeField]
        private Macro _macro = null;

        private Macro _currentInstance;


        public override string name { get { return macro != null ? macro.name : "No Macro"; } }
        public override string description { get { return _macro != null && !string.IsNullOrEmpty(_macro.comments) ? _macro.comments : base.description; } }

        public Macro macro {
            get { return _macro; }
            set
            {
                if ( _macro != value ) {
                    _macro = value;
                    if ( value != null ) { GatherPorts(); }
                }
            }
        }

        Graph IGraphAssignable.subGraph { get { return macro; } set { macro = (Macro)value; } }
        Graph IGraphAssignable.currentInstance { get { return _currentInstance; } set { _currentInstance = (Macro)value; } }
        List<NodeCanvas.Framework.Internal.BBMappingParameter> IGraphAssignable.variablesMap { get { return null; } set { /* ... */ } }
        BBParameter IGraphAssignable.subGraphParameter => null;
        Dictionary<Graph, Graph> IGraphAssignable.instances {
            get
            {
                //we only do this so that it gets cleaned up when owner is destroyed
                var dict = new Dictionary<Graph, Graph>();
                dict[_currentInstance] = _currentInstance;
                return dict;
            }
            set { throw new System.Exception("Should have not been called"); }
        }

        ///----------------------------------------------------------------------------------------------

        public void MakeInstance() {
            if ( _currentInstance == null && macro != null ) {
                _currentInstance = Graph.Clone<Macro>(macro, this.graph);
                _macro = _currentInstance;
                GatherPorts();
            }
        }

        void IUpdatable.Update() {
            if ( _currentInstance != null ) {
                _currentInstance.UpdateGraph(this.graph.deltaTime);
            }
        }

        protected override void RegisterPorts() {

            var target = _currentInstance != null ? _currentInstance : macro;

            if ( target == null || target.entry == null || target.exit == null ) {
                return;
            }

            for ( var i = 0; i < target.inputDefinitions.Count; i++ ) {
                var defIn = target.inputDefinitions[i];
                if ( defIn.type == typeof(Flow) ) {
                    AddFlowInput(defIn.name, (f) => { target.entryActionMap[defIn.ID](f); }, defIn.ID);
                } else {
                    target.entryFunctionMap[defIn.ID] = AddValueInput(defIn.name, defIn.type, defIn.ID).GetObjectValue;
                }
            }

            for ( var i = 0; i < target.outputDefinitions.Count; i++ ) {
                var defOut = target.outputDefinitions[i];
                if ( defOut.type == typeof(Flow) ) {
                    target.exitActionMap[defOut.ID] = AddFlowOutput(defOut.name, defOut.ID).Call;
                } else {
                    AddValueOutput(defOut.name, defOut.type, () => { return target.exitFunctionMap[defOut.ID](); }, defOut.ID);
                }
            }
        }
    }
}