using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion;

namespace FlowCanvas.Macros
{

    [UnityEngine.CreateAssetMenu(menuName = "ParadoxNotion/FlowCanvas/Macro Asset")]
    public class Macro : FlowScriptBase
    {

        ///----------------------------------------------------------------------------------------------
        [System.Serializable]
        class DerivedSerializationData
        {
            public List<DynamicParameterDefinition> inputDefinitions;
            public List<DynamicParameterDefinition> outputDefinitions;
        }

        public override object OnDerivedDataSerialization() {
            var data = new DerivedSerializationData();
            data.inputDefinitions = this.inputDefinitions;
            data.outputDefinitions = this.outputDefinitions;
            return data;
        }

        public override void OnDerivedDataDeserialization(object data) {
            if ( data is DerivedSerializationData ) {
                this.inputDefinitions = ( (DerivedSerializationData)data ).inputDefinitions;
                this.outputDefinitions = ( (DerivedSerializationData)data ).outputDefinitions;
            }
        }

        ///----------------------------------------------------------------------------------------------
        //we need to let Unity serialize these as to be available OnAfterDeserialize regardless or order of execution
        [SerializeField] public List<DynamicParameterDefinition> inputDefinitions = new List<DynamicParameterDefinition>();
        [SerializeField] public List<DynamicParameterDefinition> outputDefinitions = new List<DynamicParameterDefinition>();
        ///----------------------------------------------------------------------------------------------

        [NonSerialized] public Dictionary<string, FlowHandler> entryActionMap = new Dictionary<string, FlowHandler>(StringComparer.Ordinal);
        [NonSerialized] public Dictionary<string, FlowHandler> exitActionMap = new Dictionary<string, FlowHandler>(StringComparer.Ordinal);
        [NonSerialized] public Dictionary<string, ValueHandlerObject> entryFunctionMap = new Dictionary<string, ValueHandlerObject>(StringComparer.Ordinal);
        [NonSerialized] public Dictionary<string, ValueHandlerObject> exitFunctionMap = new Dictionary<string, ValueHandlerObject>(StringComparer.Ordinal);

        private MacroInputNode _entry;
        private MacroOutputNode _exit;


        ///Macros dont use blackboard overrides or blackboard variables parametrization
        public override bool allowBlackboardOverrides => false;

        ///The entry node of the Macro (input ports)
        public MacroInputNode entry {
            get
            {
                if ( _entry == null ) {
                    _entry = allNodes.OfType<MacroInputNode>().FirstOrDefault();
                    if ( _entry == null ) {
                        _entry = AddNode<MacroInputNode>(new Vector2(-translation.x + 200, -translation.y + 200));
                    }
                }
                return _entry;
            }
        }

        ///The exit node of the Macro (output ports)
        public MacroOutputNode exit {
            get
            {
                if ( _exit == null ) {
                    _exit = allNodes.OfType<MacroOutputNode>().FirstOrDefault();
                    if ( _exit == null ) {
                        _exit = AddNode<MacroOutputNode>(new Vector2(-translation.x + 600, -translation.y + 200));
                    }
                }
                return _exit;
            }
        }

        ///validates the entry and exit references
        protected override void OnGraphValidate() {
            base.OnGraphValidate();
            _entry = null;
            _exit = null;
            _entry = entry;
            _exit = exit;
        }

        ///Adds a new input port definition to the Macro
        public Port AddInputDefinition(DynamicParameterDefinition def) {
            if ( inputDefinitions.Find(d => d.ID == def.ID) == null ) {
                inputDefinitions.Add(def);
                entry.GatherPorts();
                return entry.GetOutputPort(def.ID);
            }
            return null;
        }

        ///Adds a new output port definition to the Macro
        public Port AddOutputDefinition(DynamicParameterDefinition def) {
            if ( outputDefinitions.Find(d => d.ID == def.ID) == null ) {
                outputDefinitions.Add(def);
                exit.GatherPorts();
                return exit.GetInputPort(def.ID);
            }
            return null;
        }

        //create initial example ports in case there are none in both entry and exit
        public void AddExamplePorts() {
            if ( inputDefinitions.Count == 0 && outputDefinitions.Count == 0 ) {
                var defIn = new DynamicParameterDefinition("In", typeof(Flow));
                var defOut = new DynamicParameterDefinition("Out", typeof(Flow));
                var source = AddInputDefinition(defIn);
                var target = AddOutputDefinition(defOut);
                BinderConnection.Create(source, target);
                Validate();
            }
        }

        ///----------------------------------------------------------------------------------------------

        /// Set a value input of type T of the Macro to a certain value.
        /// Only use to interface with the Macro from code.
        public void SetValueInput<T>(string name, T value) {
            var def = inputDefinitions.FirstOrDefault(d => d.name == name && d.type == typeof(T));
            if ( def == null ) {
                ParadoxNotion.Services.Logger.LogError(string.Format("Input of name {0} and type {1}, does not exist within the list of Macro Inputs", name, typeof(T)), NodeCanvas.Framework.LogTag.EXECUTION, entry);
                return;
            }
            entryFunctionMap[def.ID] = () => { return value; };
        }

        /// Call a Flow Input of the Macro.
        /// Only use to interface with the Macro from code.
        public void CallFlowInput(string name) {
            var def = inputDefinitions.FirstOrDefault(d => d.name == name && d.type == typeof(Flow));
            if ( def == null ) {
                ParadoxNotion.Services.Logger.LogError(string.Format("Input of name {0} and type Flow, does not exist within the list of Macro Inputs", name), NodeCanvas.Framework.LogTag.EXECUTION, entry);
                return;
            }
            entryActionMap[def.ID](new Flow());
        }

        /// Get the value output of type T of the Macro.
        /// Only use to interface with the Macro from code.
        public T GetValueOutput<T>(string name) {
            var def = outputDefinitions.FirstOrDefault(d => d.name == name && d.type == typeof(T));
            if ( def == null ) {
                ParadoxNotion.Services.Logger.LogError(string.Format("Input of name {0} and type {1} do not exist within the list of Macro Outputs", name, typeof(T)), NodeCanvas.Framework.LogTag.EXECUTION, exit);
                return default(T);
            }
            return (T)exitFunctionMap[def.ID]();
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/ParadoxNotion/FlowCanvas/Create/Macro Asset", false, 1)]
        public static void CreateMacro() {
            var macro = ParadoxNotion.Design.EditorUtils.CreateAsset<Macro>();
            macro.AddExamplePorts();
            UnityEditor.Selection.activeObject = macro;
            ParadoxNotion.Design.UndoUtility.SetDirty(macro);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif

        ///----------------------------------------------------------------------------------------------


    }
}