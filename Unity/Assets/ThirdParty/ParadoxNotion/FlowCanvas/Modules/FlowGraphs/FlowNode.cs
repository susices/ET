#define DO_EDITOR_BINDING //comment this out to test the real performance without editor binding specifics

#if UNITY_EDITOR
using UnityEditor;
using NodeCanvas.Editor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using Logger = ParadoxNotion.Services.Logger;
using System.Diagnostics;

namespace FlowCanvas
{

    ///The base node class for FlowGraph systems
    abstract public partial class FlowNode : Node, ISerializationCallbackReceiver
    {

        ///----------------------------------------------------------------------------------------------

        [AttributeUsage(AttributeTargets.Class)] ///Helper attribute for context menu
		public class ContextDefinedInputsAttribute : Attribute
        {
            readonly public Type[] types;
            public ContextDefinedInputsAttribute(params Type[] types) {
                this.types = types;
            }
        }

        [AttributeUsage(AttributeTargets.Class)] ///Helper attribute for context menu
		public class ContextDefinedOutputsAttribute : Attribute
        {
            readonly public Type[] types;
            public ContextDefinedOutputsAttribute(params Type[] types) {
                this.types = types;
            }
        }

        [AttributeUsage(AttributeTargets.Class)] ///Helper attribute to show refresh button
		public class HasRefreshButtonAttribute : Attribute { }

        [AttributeUsage(AttributeTargets.Field)] ///When field change, ports will be gathered
		public class GatherPortsCallbackAttribute : CallbackAttribute
        {
            public GatherPortsCallbackAttribute() : base("GatherPorts") { }
        }

        ///----------------------------------------------------------------------------------------------

        [SerializeField] //the user defined input port values
        private Dictionary<string, object> _inputPortValues;

        [NonSerialized] //all input ports
        private Dictionary<string, Port> inputPorts;
        [NonSerialized] //all output ports
        private Dictionary<string, Port> outputPorts;

        ///----------------------------------------------------------------------------------------------

        sealed public override int maxInConnections => -1;
        sealed public override int maxOutConnections => -1;
        sealed public override bool allowAsPrime => false;
        sealed public override bool canSelfConnect => false;
        sealed public override Type outConnectionType => typeof(BinderConnection);
        sealed public override Alignment2x2 commentsAlignment => Alignment2x2.Bottom;
        public override Alignment2x2 iconAlignment => Alignment2x2.Left;
        public FlowGraph flowGraph => (FlowGraph)graph;
        protected string SPACE => StringUtils.SPACE;

        ///----------------------------------------------------------------------------------------------

        ///Ignore the Self Instance Object feature altogether for the the node?
        virtual public bool ignoreSelfInstancePortAssignment => false;

        ///----------------------------------------------------------------------------------------------

        ///Store the changed input port values.
        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            if ( inputPorts != null ) {
                var result = new Dictionary<string, object>(StringComparer.Ordinal);
                var store = false;
                foreach ( var port in inputPorts.Values.OfType<ValueInput>() ) {
                    if ( !port.isConnected && !port.isDefaultValue ) {
                        store = true;
                        result[port.ID] = port.serializedValue;
                    }
                }
                _inputPortValues = store ? result : null;
            }
        }

        //Nothing... inputPortValues are already deserialized, but we need to 'load' them into the ports after GatherPorts.
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }

        ///----------------------------------------------------------------------------------------------

        ///This is called when the node is created, duplicated or otherwise needs validation
        sealed public override void OnValidate(Graph flowGraph) { GatherPorts(); }

        //Sealed for future proof use
        sealed public override void OnParentConnected(int i) { }
        sealed public override void OnChildConnected(int i) { }
        sealed public override void OnParentDisconnected(int i) { }
        sealed public override void OnChildDisconnected(int i) { }

        ///---------------------------------------------------------------------------------------------

        ///Callback when a port in this node, is connected with another port
        virtual public void OnPortConnected(Port port, Port otherPort) {
            TryHandleWildPortConnection(port.type, otherPort.type);
        }

        ///Callback when a port in this node, is disconnected from another port
        virtual public void OnPortDisconnected(Port port, Port otherPort) {
            TryRemovePortIfMissingAndClean(port);
        }

        ///---------------------------------------------------------------------------------------------

        ///Bind the port delegates. Called in runtime
        public void BindPorts() {
            for ( var i = 0; i < outConnections.Count; i++ ) {
                ( outConnections[i] as BinderConnection ).Bind();
            }
        }

        ///Unbind the ports.
        public void UnBindPorts() {
            for ( var i = 0; i < outConnections.Count; i++ ) {
                ( outConnections[i] as BinderConnection ).UnBind();
            }
        }

        ///Gets an input Port by it's ID name which commonly is the same as it's name
        public Port GetInputPort(string ID) {
            Port input = null;
            if ( inputPorts != null ) {
                if ( !inputPorts.TryGetValue(ID, out input) ) {
                    // update from previous version
                    input = inputPorts.Values.FirstOrDefault(p => CheckReverseIDEquality(p, ID));
                }
            }
            return input;
        }

        ///Gets an output Port by it's ID name which commonly is the same as it's name
        public Port GetOutputPort(string ID) {
            Port output = null;
            if ( outputPorts != null ) {
                if ( !outputPorts.TryGetValue(ID, out output) ) {
                    // update from previous version
                    output = outputPorts.Values.FirstOrDefault(p => CheckReverseIDEquality(p, ID));
                }
            }
            return output;
        }

        ///----------------------------------------------------------------------------------------------

        ///Returns all ports
        public IEnumerable<Port> GetAllPorts() {
            return inputPorts.Values.Concat(outputPorts.Values);
        }

        ///Returns all Flow Output Ports
        public IEnumerable<FlowOutput> GetOutputFlowPorts() {
            return outputPorts.Values.OfType<FlowOutput>();
        }

        ///Returns all Value Output Ports
        public IEnumerable<ValueOutput> GetOutputValuePorts() {
            return outputPorts.Values.OfType<ValueOutput>();
        }

        ///Returns all Flow Input Ports
        public IEnumerable<FlowInput> GetInputFlowPorts() {
            return inputPorts.Values.OfType<FlowInput>();
        }

        ///Returns all Value Input Ports
        public IEnumerable<ValueInput> GetInputValuePorts() {
            return inputPorts.Values.OfType<ValueInput>();
        }

        ///----------------------------------------------------------------------------------------------

        ///Returns the first input port assignable to the type provided
        public Port GetFirstInputOfType(Type type) {
            return inputPorts.Values.OrderBy(p => p is FlowInput ? 0 : 1).FirstOrDefault(p => p.type.RTIsAssignableFrom(type));
        }

        ///Returns the first output port of a type assignable to the port
        public Port GetFirstOutputOfType(Type type) {
            return outputPorts.Values.OrderBy(p => p is FlowInput ? 0 : 1).FirstOrDefault(p => type.RTIsAssignableFrom(p.type));
        }

        ///----------------------------------------------------------------------------------------------

        ///Set the Component or GameObject instance input port to Owner if not connected and not already set to another reference.
        ///By convention, the instance port is always considered to be the first.
        ///Called from Graph when started.
        public void AssignSelfInstancePort() {
            if ( ignoreSelfInstancePortAssignment ) { return; }
            var instanceInput = inputPorts.Values.OfType<ValueInput>().FirstOrDefault(p => p.IsUnitySceneObject() && !p.skipSelfInstanceAssignment);
            if ( instanceInput != null && !instanceInput.isConnected && instanceInput.isDefaultValue ) {
                var instance = flowGraph.GetAgentComponent(instanceInput.type);
                if ( instance != null ) { instanceInput.serializedValue = instance; }
            }
        }

        ///Gather and register the node ports.
        public void GatherPorts() {
            inputPorts = new Dictionary<string, Port>(StringComparer.Ordinal);
            outputPorts = new Dictionary<string, Port>(StringComparer.Ordinal);
            try {
                RegisterPorts();
                ValidateConnections();
            }
            finally {
                DeserializeInputPortValues();
                RefreshEditorPorts();
            }
        }

        ///Override for registration/definition of ports.
        abstract protected void RegisterPorts();

        //Validate ports for connections
        //This is done seperately for Source and Target since we don't get control of when GatherPorts will be called on each node apart from in order of list (and we dont care)
        //So basicaly each node validates it's own inputs and outputs seperately.
        [Conditional("UNITY_EDITOR")]
        void ValidateConnections() {
#if UNITY_EDITOR && DO_EDITOR_BINDING

            for ( var i = outConnections.Count; i-- > 0; ) { //reverse cause may remove
                var cOut = outConnections[i] as BinderConnection;
                if ( cOut != null ) { cOut.GatherAndValidateSourcePort(); }
            }

            for ( var i = inConnections.Count; i-- > 0; ) { //reverse cause may remove
                var cIn = inConnections[i] as BinderConnection;
                if ( cIn != null ) { cIn.GatherAndValidateTargetPort(); }
            }

            //keep connections in same order as their respective ports for consistency? Too slow for no reason?
            // inConnections = inConnections.OfType<BinderConnection>().OrderBy(c => inputPorts.Values.ToList().IndexOf(c.targetPort) ).Cast<Connection>().ToList();
            // outConnections = outConnections.OfType<BinderConnection>().OrderBy(c => outputPorts.Values.ToList().IndexOf(c.sourcePort) ).Cast<Connection>().ToList();
#endif
        }

        ///Restore the serialized input port values
        void DeserializeInputPortValues() {

            if ( _inputPortValues == null ) {
                return;
            }

            foreach ( var pair in _inputPortValues ) {
                Port inputPort = null;
                if ( !inputPorts.TryGetValue(pair.Key, out inputPort) ) {
                    // update from previous version
                    inputPort = inputPorts.Values.FirstOrDefault(p => CheckReverseIDEquality(p, pair.Key));
                }

                if ( inputPort is ValueInput && pair.Value != null && inputPort.type.RTIsAssignableFrom(pair.Value.GetType()) ) {
                    ( inputPort as ValueInput ).serializedValue = pair.Value;
                }
            }
        }

        [Conditional("UNITY_EDITOR")]
        void RefreshEditorPorts() {
#if UNITY_EDITOR && DO_EDITOR_BINDING
            OnPortsGatheredInEditor();
#endif
        }

        ///---------------------------------------------------------------------------------------------
        //Port registration/definition methods, to be used within RegisterPorts override

        ///Add a new FlowInput with name and pointer. Pointer is the method to run when the flow port is called. Returns the new FlowInput object.
        public FlowInput AddFlowInput(string name, string ID, FlowHandler pointer) { return AddFlowInput(name, pointer, ID); }
        public FlowInput AddFlowInput(string name, FlowHandler pointer, string ID = "") {
            QualifyPortNameAndID(ref name, ref ID, inputPorts);
            return (FlowInput)( inputPorts[ID] = new FlowInput(this, name, ID, pointer) );
        }

        ///Add a new FlowOutput with name. Returns the new FlowOutput object.
        public FlowOutput AddFlowOutput(string name, string ID = "") {
            QualifyPortNameAndID(ref name, ref ID, outputPorts);
            return (FlowOutput)( outputPorts[ID] = new FlowOutput(this, name, ID) );
        }

        ///Recommended. Add a ValueInput of type T. Returns the new ValueInput<T> object.
        public ValueInput<T> AddValueInput<T>(string name, string ID = "") {
            QualifyPortNameAndID(ref name, ref ID, inputPorts);
            return (ValueInput<T>)( inputPorts[ID] = new ValueInput<T>(this, name, ID) );
        }

        ///Recommended. Add a ValueOutput of type T. getter is the function to get the value from. Returns the new ValueOutput<T> object.
        public ValueOutput<T> AddValueOutput<T>(string name, string ID, ValueHandler<T> getter) { return AddValueOutput<T>(name, getter, ID); }
        public ValueOutput<T> AddValueOutput<T>(string name, ValueHandler<T> getter, string ID = "") {
            QualifyPortNameAndID(ref name, ref ID, outputPorts);
            return (ValueOutput<T>)( outputPorts[ID] = new ValueOutput<T>(this, name, ID, getter) );
        }

        ///Add an object port of unkown runtime type. getter is a function to get the port value from. Returns the new ValueOutput object.
        ///It is always recommended to use the generic versions to avoid value boxing/unboxing!
        public ValueInput AddValueInput(string name, string ID, Type type) { return AddValueInput(name, type, ID); }
        public ValueInput AddValueInput(string name, Type type, string ID = "") {
            QualifyPortNameAndID(ref name, ref ID, inputPorts);
            return (ValueInput)( inputPorts[ID] = ValueInput.CreateInstance(type, this, name, ID) );
        }

        ///Add an object port of unkown runtime type. getter is a function to get the port value from. Returns the new ValueOutput object.
        ///It is always recommended to use the generic versions to avoid value boxing/unboxing!
        public ValueOutput AddValueOutput(string name, string ID, Type type, ValueHandlerObject getter) { return AddValueOutput(name, type, getter, ID); }
        public ValueOutput AddValueOutput(string name, Type type, ValueHandlerObject getter, string ID = "") {
            QualifyPortNameAndID(ref name, ref ID, outputPorts);
            return (ValueOutput)( outputPorts[ID] = ValueOutput.CreateInstance(type, this, name, ID, getter) );
        }

        ///Helper used above
        void QualifyPortNameAndID(ref string name, ref string ID, IDictionary dict) {
            if ( string.IsNullOrEmpty(ID) ) ID = name;
            if ( string.IsNullOrEmpty(ID) ) {
                ID = SPACE;
                while ( dict.Contains(ID) ) {
                    ID += SPACE;
                }
            }
        }

        //Check whether port can qualify for requested ID
        //This is only for upgrading from previous versions.
        bool CheckReverseIDEquality(Port port, string requestedID) {
            if ( port.ID.Trim() == requestedID.Trim() ) { return true; }
            if ( port.name.Trim() == requestedID.Trim() ) { return true; }
            if ( port.name.SplitCamelCase().Trim() == requestedID.Trim() ) { return true; }
            return false;
        }

        ///----------------------------------------------------------------------------------------------

        ///Reflection based registration for target object.
        ///This is nowhere used by default.
        public void TryAddReflectionBasedRegistrationForObject(object instance) {
            //FlowInputs. All public void methods with one Flow parameter.
            foreach ( var method in instance.GetType().RTGetMethods() ) {
                if ( method.IsPublic ) { TryAddMethodFlowInput(method, instance); }
            }

            //ValueOutputs. All readable public properties.
            foreach ( var prop in instance.GetType().RTGetProperties() ) {
                if ( prop.GetGetMethod().IsPublic ) { TryAddPropertyValueOutput(prop, instance); }
            }

            //FlowOutputs. All public FlowHandler delegate fields.
            //ValueInputs. All public ValueHandler delegate fields.
            foreach ( var field in instance.GetType().RTGetFields() ) {
                if ( field.IsPublic ) {
                    TryAddFieldDelegateFlowOutput(field, instance);
                    TryAddFieldDelegateValueInput(field, instance);
                }
            }
        }

        ///Register a MethodInfo as FlowInput. Used only in reflection based registration.
        public FlowInput TryAddMethodFlowInput(MethodInfo method, object instance) {
            var parameters = method.GetParameters();
            if ( method.ReturnType == typeof(void) && parameters.Length == 1 && parameters[0].ParameterType == typeof(Flow) ) {
                var nameAtt = method.RTGetAttribute<NameAttribute>(true);
                var name = nameAtt != null ? nameAtt.name : method.Name;
                var pointer = method.RTCreateDelegate<FlowHandler>(instance);
                return AddFlowInput(name, pointer);
            }
            return null;
        }

        ///Register a FieldInfo Delegate (FlowHandler) as FlowOutput. Used only in reflection based registration.
        public FlowOutput TryAddFieldDelegateFlowOutput(FieldInfo field, object instance) {
            if ( field.FieldType == typeof(FlowHandler) ) {
                var nameAtt = field.RTGetAttribute<NameAttribute>(true);
                var name = nameAtt != null ? nameAtt.name : field.Name;
                var flowOut = AddFlowOutput(name);
                field.SetValue(instance, (FlowHandler)flowOut.Call);
                return flowOut;
            }
            return null;
        }

        ///Register a FieldInfo Delegate (ValueHandler<T>) as ValueInput. Used only in reflection based registration.
        public ValueInput TryAddFieldDelegateValueInput(FieldInfo field, object instance) {
            if ( typeof(Delegate).RTIsAssignableFrom(field.FieldType) ) {
                var invokeMethod = field.FieldType.RTGetMethod("Invoke");
                var parameters = invokeMethod.GetParameters();
                if ( invokeMethod.ReturnType != typeof(void) && parameters.Length == 0 ) {
                    var nameAtt = field.RTGetAttribute<NameAttribute>(true);
                    var name = nameAtt != null ? nameAtt.name : field.Name;
                    var delType = invokeMethod.ReturnType;
                    var portType = typeof(ValueInput<>).RTMakeGenericType(delType);
                    var port = (ValueInput)Activator.CreateInstance(portType, new object[] { instance, name, name });
                    var getterType = typeof(ValueHandler<>).RTMakeGenericType(delType);
                    var getter = port.GetType().RTGetMethod("get_value").RTCreateDelegate(getterType, port);
                    field.SetValue(instance, getter);
                    inputPorts[name] = port;
                    return port;
                }
            }
            return null;
        }

        ///Register a PropertyInfo as ValueOutput. Used only in reflection based registration.
        public ValueOutput TryAddPropertyValueOutput(PropertyInfo prop, object instance) {
            if ( prop.CanRead ) {
                var nameAtt = prop.RTGetAttribute<NameAttribute>(true);
                var name = nameAtt != null ? nameAtt.name : prop.Name;
                var getterType = typeof(ValueHandler<>).RTMakeGenericType(prop.PropertyType);
                var getter = prop.RTGetGetMethod().RTCreateDelegate(getterType, instance);
                var portType = typeof(ValueOutput<>).RTMakeGenericType(prop.PropertyType);
                var port = (ValueOutput)Activator.CreateInstance(portType, new object[] { this, name, name, getter });
                return (ValueOutput)( outputPorts[name] = port );
            }
            return null;
        }

        ///----------------------------------------------------------------------------------------------

        ///Replace with another type.
        //1) Because SetTarget, SetSource also fires OnPortConnected, Wild ports are handled automatically.
        //2) GatherPorts is also firing connections validation.
        //3) Because connections are validated, changing connection types to correct types is also automatic.
        //So for example if we change to a new Wild type, OnPortConnected will re-set the type to closed.
        public FlowNode ReplaceWith(System.Type t) {
            var newNode = graph.AddNode(t, this.position) as FlowNode;
            if ( newNode == null ) { return null; }
            foreach ( var c in inConnections.ToArray() ) {
                c.SetTargetNode(newNode);
            }
            foreach ( var c in outConnections.ToArray() ) {
                c.SetSourceNode(newNode);
            }
            if ( this._inputPortValues != null ) {
                newNode._inputPortValues = this._inputPortValues.ToDictionary(k => k.Key, v => v.Value);
            }
            graph.RemoveNode(this);
            newNode.GatherPorts();
            return (FlowNode)newNode;
        }

        ///Try remove port if it's missing and has no connections left
        void TryRemovePortIfMissingAndClean(Port port) {
            if ( port.connections == 0 ) {
                if ( port.bindStatus == Port.BindStatus.Missing ) {
                    if ( port.IsInputPort() ) { inputPorts.Remove(port.ID); }
                    if ( port.IsOutputPort() ) { outputPorts.Remove(port.ID); }
                    RefreshEditorPorts();
                }
                if ( port.bindStatus == Port.BindStatus.InvalidCast ) {
                    port.FlagValid();
                    RefreshEditorPorts();
                }
            }
        }

        ///----------------------------------------------------------------------------------------------

        ///Should return the base wild definition type with which new generic version can be made.
        virtual public Type GetNodeWildDefinitionType() {
            return this.GetType().GetFirstGenericParameterConstraintType();
        }

        ///Handles connecting to a wild port and changing generic version to that new connection
        void TryHandleWildPortConnection(Type currentType, Type targetType) {
            var wildType = GetNodeWildDefinitionType();
            var content = this.GetType();
            var newType = TryGetNewGenericTypeForWild(wildType, currentType, targetType, content, null);
            if ( newType != null ) {
                this.ReplaceWith(newType);
            }
        }

        ///Given a wildType and two types for current and target, will try and return a closed type for wild definition of content
        public static Type TryGetNewGenericTypeForWild(Type wildType, Type currentType, Type targetType, Type content, Type context) {
            if ( wildType == null || !content.IsGenericType ) {
                return null;
            }
            var args = content.RTGetGenericArguments();
            var arg1 = args.FirstOrDefault();
            if ( arg1 != wildType && arg1.IsGenericType ) {
                return TryGetNewGenericTypeForWild(wildType, currentType, targetType, arg1, content);
            }
            //Current arg type is wild type
            if ( args.Length == 1 && arg1 == wildType ) {
                var otherPortElementType = targetType.GetEnumerableElementType();
                var portElementType = currentType.GetEnumerableElementType();
                var areBothEnumerable = otherPortElementType != null && portElementType != null;
                currentType = areBothEnumerable ? portElementType : currentType;
                targetType = areBothEnumerable ? otherPortElementType : targetType;
                //currentType is wild type, but only proceed if the target type is not
                if ( currentType == wildType && targetType != currentType ) {
                    Type newType = null;
                    if ( content.TryMakeGeneric(targetType, out newType) ) {
                        if ( context.TryMakeGeneric(newType, out Type contextType) ) {
                            newType = contextType;
                        }
                        return newType;
                    }
                }
            }
            return null;
        }

        ///Given a wildType and two types for current and target, will try and return a closed method for wild definition of content
        public static MethodInfo TryGetNewGenericMethodForWild(Type wildType, Type currentType, Type targetType, MethodInfo content) {
            if ( wildType == null || !content.IsGenericMethod ) {
                return null;
            }
            var args = content.RTGetGenericArguments();
            var arg1 = args.FirstOrDefault();
            if ( args.Length == 1 && arg1 == wildType ) {
                var otherPortElementType = targetType.GetEnumerableElementType();
                var portElementType = currentType.GetEnumerableElementType();
                var areBothEnumerable = otherPortElementType != null && portElementType != null;
                currentType = areBothEnumerable ? portElementType : currentType;
                targetType = areBothEnumerable ? otherPortElementType : targetType;
                if ( currentType == wildType && targetType != currentType ) {
                    if ( content.TryMakeGeneric(targetType, out MethodInfo genericMethod) ) {
                        return genericMethod;
                    }
                }
            }
            return null;
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
        ///----------------------------------------------------------------------------------------------
#if UNITY_EDITOR

        private Port[] orderedInputs;
        private Port[] orderedOutputs;
        private ValueInput selfInstancePort;
        private bool highlightFlag;
        private bool portsHaveDrawnFlag;

        private static int dragDropMisses;
        private static bool popKeyDown;

        //the current clicked port when creating new links
        private static Port _clickedPort;
        private static Port clickedPort {
            get { return _clickedPort; }
            set
            {
                _clickedPort = value;
                contextPort = value;
            }
        }

        //the current relink binder if any
        private static BinderConnection _relinkBinder;
        private static BinderConnection relinkBinder {
            get { return _relinkBinder; }
            set
            {
                _relinkBinder = value;
                if ( value != null ) {
                    if ( value.relinkState == Connection.RelinkState.Target ) {
                        contextPort = value.sourcePort;
                    }
                    if ( value.relinkState == Connection.RelinkState.Source ) {
                        contextPort = value.targetPort;
                    }
                } else {
                    contextPort = null;
                }
            }
        }

        //the current context port if any for linking relinking
        private static Port _contextPort;
        private static Port contextPort {
            get { return _contextPort; }
            set
            {
                var graph = value != null ? value.parent.graph : ( _contextPort != null ? _contextPort.parent.graph : null );
                popKeyDown = false;
                _contextPort = value;
                if ( graph != null ) {
                    for ( var i = 0; i < graph.allNodes.Count; i++ ) {
                        var node = graph.allNodes[i] as FlowNode;
                        if ( node != null ) {
                            node.highlightFlag = false;
                            for ( var j = 0; j < node.orderedInputs.Length; j++ ) {
                                var port = node.orderedInputs[j];
                                port.highlightFlag = value != null && value.IsOutputPort() ? BinderConnection.CanBeBound(value, port, relinkBinder) : false;
                                node.highlightFlag |= port.highlightFlag;
                            }
                            for ( var j = 0; j < node.orderedOutputs.Length; j++ ) {
                                var port = node.orderedOutputs[j];
                                port.highlightFlag = value != null && value.IsInputPort() ? BinderConnection.CanBeBound(port, value, relinkBinder) : false;
                                node.highlightFlag |= port.highlightFlag;
                            }
                        }
                    }
                }
            }
        }

        //when gathering ports and we are in Unity Editor
        //gather the ordered inputs and outputs
        void OnPortsGatheredInEditor() {
            portsHaveDrawnFlag = false;
            orderedInputs = inputPorts.Values.OrderBy(p => p is FlowInput ? 0 : 1).ToArray();
            orderedOutputs = outputPorts.Values.OrderBy(p => p is FlowOutput ? 0 : ( p.IsDelegate() ? -1 : 1 )).ToArray();
            selfInstancePort = ignoreSelfInstancePortAssignment ? null : orderedInputs.OfType<ValueInput>().FirstOrDefault(p => p.IsUnitySceneObject() && !p.skipSelfInstanceAssignment);
        }

        //Seal it...
        sealed protected override void DrawNodeConnections(Rect drawCanvas, bool fullDrawPass, Vector2 canvasMousePos, float zoomFactor) {

            var e = Event.current;

            if ( fullDrawPass || drawCanvas.Overlaps(rect) ) {

                DoVerboseLevelGUI(e);

                if ( highlightFlag ) {
                    Styles.Draw(rect, Styles.highlightBox);
                }

                //Port container graphics
                GUI.Box(new Rect(rect.x - 8, rect.y + 2, 10, rect.height), string.Empty, StyleSheet.nodePortContainer);
                GUI.Box(new Rect(rect.xMax - 2, rect.y + 2, 10, rect.height), string.Empty, StyleSheet.nodePortContainer);

                //INPUT Ports
                if ( orderedInputs != null ) {
                    for ( var i = 0; i < orderedInputs.Length; i++ ) {
                        var port = orderedInputs[i];
                        port.willDraw = ShouldDrawPort(port, popKeyDown);
                        if ( port.willDraw && portsHaveDrawnFlag ) {
                            DrawInputPortGraphic(port, contextPort, port == selfInstancePort);
                            HandlePortEvents(port);
                        }
                    }
                }

                //OUTPUT Ports
                if ( orderedOutputs != null ) {
                    for ( var i = 0; i < orderedOutputs.Length; i++ ) {
                        var port = orderedOutputs[i];
                        port.willDraw = ShouldDrawPort(port, popKeyDown);
                        if ( port.willDraw && portsHaveDrawnFlag ) {
                            DrawOutputPortGraphic(port, contextPort);
                            HandlePortEvents(port);
                        }
                    }
                }
            }

            ///ACCEPT CONNECTION
            if ( clickedPort != null && e.type == EventType.MouseUp ) {

                ///ON NODE
                if ( rect.Contains(e.mousePosition) ) {
                    var cachePort = clickedPort;
                    clickedPort = null;
                    DoDropInConnectionMenu(cachePort);
                    e.Use();

                    ///ON CANVAS
                } else {

                    dragDropMisses++;
                    if ( dragDropMisses == graph.allNodes.Count && clickedPort != null ) {
                        var cachePort = clickedPort;
                        clickedPort = null;

                        var mPos = e.mousePosition;
                        if ( zoomFactor >= 1 ) {
                            GenericMenuBrowser.ShowAsync(mPos, string.Format("Add & Connect (Type of {0})", cachePort.type.FriendlyName()), graph.baseNodeType,
                            () => { return flowGraph.GetFullNodesMenu(mPos, cachePort, null); });
                        } else {
                            EditorApplication.delayCall += () =>
                            {
                                GenericMenuBrowser.ShowAsync(GraphEditorUtility.realMousePosition, string.Format("Add & Connect (Type of {0})", cachePort.type.FriendlyName()), graph.baseNodeType,
                                () => { return flowGraph.GetFullNodesMenu(mPos, cachePort, null); });
                            };
                        }

                        e.Use();
                    }
                }
            }

            //Temp connection line when linking
            if ( clickedPort != null && clickedPort.parent == this ) {
                var from = clickedPort.IsOutputPort() ? clickedPort.pos : e.mousePosition;
                var to = clickedPort.IsOutputPort() ? e.mousePosition : clickedPort.pos;
                var tangA = default(Vector2);
                var tangB = default(Vector2);
                ParadoxNotion.CurveUtils.ResolveTangents(from, to, Prefs.connectionsMLT, PlanarDirection.Horizontal, out tangA, out tangB);
                Handles.DrawBezier(from, to, from + tangA, to + tangB, StyleSheet.GetStatusColor(Status.Resting).WithAlpha(0.5f), StyleSheet.bezierTexture, 3);
            }

            //Actualy draw the existing connections
            for ( var i = 0; i < outConnections.Count; i++ ) {
                var binder = outConnections[i] as BinderConnection;
                if ( binder != null ) { //for in case it's MissingConnection
                    var sourcePort = binder.sourcePort;
                    var targetPort = binder.targetPort;

                    var sourcePos = sourcePort != null ? sourcePort.pos : new Vector2(binder.sourceNode.rect.xMax + 10, binder.sourceNode.rect.y + 10);
                    var targetPos = targetPort != null ? targetPort.pos : new Vector2(binder.targetNode.rect.xMin - 10, binder.targetNode.rect.y + 10);
                    if ( fullDrawPass || drawCanvas.Overlaps(RectUtils.GetBoundRect(sourcePos, targetPos)) ) {
                        //this is just to avoid connections glitching on repaint which drives me crazy
                        if ( !fullDrawPass && ( !portsHaveDrawnFlag || ( targetPort != null && !targetPort.parent.portsHaveDrawnFlag ) ) ) { continue; }
                        binder.DrawConnectionGUI(sourcePos, targetPos);
                    }
                    continue;
                }
                if ( binder == null && !( binder.targetNode is NodeCanvas.Framework.Internal.MissingNode ) ) {
                    outConnections[i].DrawConnectionGUI(outConnections[i].sourceNode.rect.center, outConnections[i].targetNode.rect.center);
                }
            }
        }

        ///button and shortcuts to change verbose level
        void DoVerboseLevelGUI(Event e) {

            if ( GUIUtility.keyboardControl == 0 ) {
                if ( e.type == EventType.KeyDown && e.keyCode == KeyCode.S && !e.control ) { popKeyDown = true; e.Use(); }
                if ( e.rawType == EventType.KeyUp && e.keyCode == KeyCode.S ) { popKeyDown = false; }
            }

            if ( GraphEditorUtility.allowClick && rect.ExpandBy(0, 25, 0, 0).Contains(e.mousePosition) ) {
                GUI.BringWindowToFront(ID);
                var verboseIconRect = new Rect(rect.xMax - 18, rect.yMin - 18, 16, 16);
                Texture2D image = null;
                if ( verboseLevel == Node.VerboseLevel.Compact ) { image = StyleSheet.verboseLevel1; }
                if ( verboseLevel == Node.VerboseLevel.Partial ) { image = StyleSheet.verboseLevel2; }
                if ( verboseLevel == Node.VerboseLevel.Full ) { image = StyleSheet.verboseLevel3; }
                EditorGUIUtility.AddCursorRect(verboseIconRect, MouseCursor.Link);
                GUI.color = Colors.Grey(0.5f);
                if ( GUI.Button(verboseIconRect, image, GUIStyle.none) ) {
                    verboseLevel = (Node.VerboseLevel)Mathf.Repeat(( (int)verboseLevel ) - 1, 3);
                }
                GUI.color = Color.white;

                if ( EditorGUIUtility.keyboardControl == 0 && e.type == EventType.KeyDown ) {
                    if ( e.keyCode == KeyCode.Alpha1 ) { verboseLevel = Node.VerboseLevel.Compact; }
                    if ( e.keyCode == KeyCode.Alpha2 ) { verboseLevel = Node.VerboseLevel.Partial; }
                    if ( e.keyCode == KeyCode.Alpha3 ) { verboseLevel = Node.VerboseLevel.Full; }
                }
            }
        }

        //Should the port be visible? (based on node verbose level)
        static bool ShouldDrawPort(Port port, bool pop) {

            if ( port == contextPort ) {
                return true;
            }

            if ( pop || contextPort != null ) {

                if ( GraphEditorUtility.allowClick && port.parent.rect.ExpandBy(30, 40, 30, 5).Contains(Event.current.mousePosition) ) {

                    if ( contextPort != null && contextPort.parent != port.parent ) {
                        if ( port.IsInputPort() ) {
                            if ( BinderConnection.CanBeBound(contextPort, port, relinkBinder) ) {
                                return true;
                            }
                        }
                        if ( port.IsOutputPort() ) {
                            if ( BinderConnection.CanBeBound(port, contextPort, relinkBinder) ) {
                                return true;
                            }
                        }
                    }

                    if ( contextPort == null || contextPort.parent == port.parent ) {
                        return true;
                    }
                }
            }

            if ( port.parent.verboseLevel == Node.VerboseLevel.Full ) { return true; }
            if ( port.parent.verboseLevel == Node.VerboseLevel.Partial ) {
                if ( port.isConnected ) { return true; }
                if ( port is ValueInput ) { return !( port as ValueInput ).isDefaultValue; }
            }

            return false;
        }

        ///Handle relink start port to port
        public override void OnActiveRelinkStart(Connection connection) {
            relinkBinder = connection as BinderConnection;
        }

        ///Handle relink end port to port
        public override void OnActiveRelinkEnd(Connection connection) {
            var canvasMousePos = Event.current.mousePosition;
            var binder = (BinderConnection)connection;
            for ( var i = 0; i < graph.allNodes.Count; i++ ) {
                var otherNode = graph.allNodes[i] as FlowNode;

                if ( binder.relinkState == Connection.RelinkState.Target ) {

                    if ( otherNode.rect.Contains(canvasMousePos) ) {
                        otherNode.DoDropInConnectionMenu(binder.sourcePort, binder);
                        break;
                    }

                    for ( var j = 0; j < otherNode.orderedInputs.Length; j++ ) {
                        var inputPort = otherNode.orderedInputs[j];
                        if ( inputPort.rect.Contains(canvasMousePos) ) {
                            if ( !BinderConnection.CanBeBoundVerbosed(binder.sourcePort, inputPort, binder, out string verbose) ) {
                                Logger.Log(verbose, LogTag.EDITOR, otherNode);
                                break;
                            }
                            binder.SetTargetPort(inputPort);
                            break;
                        }
                    }
                }

                if ( binder.relinkState == Connection.RelinkState.Source ) {

                    if ( otherNode.rect.Contains(canvasMousePos) ) {
                        otherNode.DoDropInConnectionMenu(binder.targetPort, binder);
                        break;
                    }

                    for ( var j = 0; j < otherNode.orderedOutputs.Length; j++ ) {
                        var outputPort = otherNode.orderedOutputs[j];
                        if ( outputPort.rect.Contains(canvasMousePos) ) {
                            if ( !BinderConnection.CanBeBoundVerbosed(outputPort, binder.targetPort, binder, out string verbose) ) {
                                Logger.Log(verbose, LogTag.EDITOR, otherNode);
                                break;
                            }
                            binder.SetSourcePort(outputPort);
                            break;
                        }
                    }
                }
            }

            relinkBinder = null;
        }

        ///Handle port events
        void HandlePortEvents(Port port) {
            var e = Event.current;
            if ( GraphEditorUtility.allowClick ) {
                //Right click removes connections
                if ( port.isConnected && e.type == EventType.ContextClick && port.rect.Contains(e.mousePosition) ) {
                    foreach ( var c in port.GetPortConnections().ToArray() ) { graph.RemoveConnection(c); }
                    e.Use();
                    return;
                }

                //Click initialize new drag & drop connection
                if ( e.type == EventType.MouseDown && e.button == 0 && port.rect.Contains(e.mousePosition) ) {
                    if ( port.CanAcceptConnections() ) {
                        dragDropMisses = 0;
                        clickedPort = port;
                        e.Use();
                    }
                }

                //Drop on creates connection
                if ( e.type == EventType.MouseUp && e.button == 0 && clickedPort != null && port.rect.Contains(e.mousePosition) ) {
                    if ( port.IsInputPort() ) {
                        BinderConnection.Create(clickedPort, port);
                    }
                    if ( port.IsOutputPort() ) {
                        BinderConnection.Create(port, clickedPort);
                    }
                    clickedPort = null;
                    e.Use();
                }
            }
        }

        ///A menu for drop in connection request from provided port
        void DoDropInConnectionMenu(Port contextPort, BinderConnection refBinder = null) {
            var menu = GetDropInConnectionMenu(contextPort, refBinder);
            if ( menu.GetItemCount() == 0 ) {
                Logger.Log("No possible ports to connect to in this node.", LogTag.EDITOR, this);
                return;
            }
            if ( menu.GetItemCount() == 1 ) {
                EditorUtils.GetMenuItems(menu)[0].func();
                return;
            }
            GraphEditorUtility.PostGUI += () => { menu.ShowAsContext(); };
        }

        ///A menu for drop in connection request from provided port
        GenericMenu GetDropInConnectionMenu(Port contextPort, BinderConnection refBinder = null) {
            var menu = new GenericMenu();
            if ( contextPort.IsInputPort() ) {
                if ( orderedOutputs != null ) {
                    foreach ( var _port in orderedOutputs.Where(p => BinderConnection.CanBeBound(p, contextPort, refBinder)) ) {
                        var port = _port;
                        menu.AddItem(new GUIContent(string.Format("From: '{0}'", port.displayName)), false, () =>
                       {
                           if ( refBinder != null ) {
                               refBinder.SetSourcePort(port);
                           } else {
                               BinderConnection.Create(port, contextPort);
                           }
                       });
                    }
                }
            }
            if ( contextPort.IsOutputPort() ) {
                if ( orderedInputs != null ) {
                    foreach ( var _port in orderedInputs.Where(p => BinderConnection.CanBeBound(contextPort, p, refBinder)) ) {
                        var port = _port;
                        menu.AddItem(new GUIContent(string.Format("To: '{0}'", port.displayName)), false, () =>
                       {
                           if ( refBinder != null ) {
                               refBinder.SetTargetPort(port);
                           } else {
                               BinderConnection.Create(contextPort, port);
                           }
                       });
                    }
                }
            }

            //append menu items
            menu = OnDragAndDropPortContextMenu(menu, contextPort);
            return menu;
        }

        ///Let nodes handle ports draged on top of them
        virtual protected GenericMenu OnDragAndDropPortContextMenu(GenericMenu menu, Port port) { return menu; }

        //Input port graphics outside node
        static void DrawInputPortGraphic(Port port, Port contextPort, bool isSelfInstancePort) {

            var canConnect = contextPort == null || port.highlightFlag;
            GUI.color = Color.white.WithAlpha(canConnect || contextPort == port ? 1 : 0.3f);
            GUI.Box(port.rect, string.Empty, port.isConnected ? StyleSheet.nodePortConnected : StyleSheet.nodePortEmpty);
            GUI.color = Color.white;

            //Tooltip
            if ( !port.isConnected && port.rect.Contains(Event.current.mousePosition) ) {
                var labelString = ( canConnect || port == contextPort ) ? port.type.FriendlyName() : "Can't Connect Here";
                var size = StyleSheet.box.CalcSize(EditorUtils.GetTempContent(labelString));
                var rect = new Rect(0, 0, size.x + 10, size.y + 5);
                rect.x = port.rect.x - size.x - 10;
                rect.y = port.rect.y - size.y / 2;
                GUI.Box(rect, labelString, StyleSheet.box);
                return;
            }

            //Or value tip
            if ( !port.isConnected && port is ValueInput ) {
                var value = ( port as ValueInput ).serializedValue;
                string labelString = null;
                if ( !( port as ValueInput ).isDefaultValue ) {
                    //Not a defaultValue
                    if ( value is UnityEngine.Object && value as UnityEngine.Object != null ) {
                        labelString = string.Format("<b><color=#66ff33>{0}</color></b>", ( value as UnityEngine.Object ).name);
                    } else {
                        labelString = value.ToStringAdvanced();
                        if ( labelString == port.type.ToString() ) {
                            labelString = null;
                        }
                    }
                } else if ( isSelfInstancePort ) {
                    //"Self"
                    var exists = true;
                    if ( port.parent.graphAgent != null && typeof(Component).IsAssignableFrom(port.type) ) {
                        exists = port.parent.graphAgent.GetComponent(port.type) != null;
                    }
                    var color = exists ? "66ff33" : "ff3300";
                    labelString = string.Format("<color=#{0}><b><i>Self</i></b></color>", color);
                } else {
                    //default value semi-transparent
                    GUI.color = Color.white.WithAlpha(0.15f);
                    labelString = value.ToStringAdvanced();
                }
                //cap string length
                if ( port.type == typeof(string) ) {
                    labelString = labelString.CapLength(20);
                }
                //show tip label
                if ( !string.IsNullOrEmpty(labelString) ) {
                    var size = StyleSheet.labelOnCanvas.CalcSize(EditorUtils.GetTempContent(labelString));
                    var rect = new Rect(0, 0, size.x, size.y);
                    rect.x = port.rect.x - size.x - 5;
                    rect.center = new Vector2(rect.center.x, port.rect.center.y - 1);
                    GUI.Label(rect, labelString, StyleSheet.labelOnCanvas);
                }
                GUI.color = Color.white;
            }
        }

        //Output port graphics outside node
        static void DrawOutputPortGraphic(Port port, Port contextPort) {

            var canConnect = contextPort == null || port.highlightFlag;
            GUI.color = Color.white.WithAlpha(canConnect || contextPort == port ? 1 : 0.3f);
            GUI.Box(port.rect, string.Empty, port.isConnected ? StyleSheet.nodePortConnected : StyleSheet.nodePortEmpty);
            GUI.color = Color.white;

            if ( !port.isConnected && port.rect.Contains(Event.current.mousePosition) ) {
                var labelString = ( canConnect || port == contextPort ) ? port.type.FriendlyName() : "Can't Connect Here";
                var size = StyleSheet.box.CalcSize(EditorUtils.GetTempContent(labelString));
                var rect = new Rect(0, 0, size.x + 10, size.y + 5);
                rect.x = port.rect.x + 15;
                rect.y = port.rect.y - port.rect.height / 2;
                GUI.Box(rect, labelString, StyleSheet.box);
            }
        }


        ///----------------------------------------------------------------------------------------------


        //GUI within the node
        protected override void OnNodeGUI() {
            // ShowWildTypeSelectionButton();
            DrawNodePorts();
        }

        //..
        // void ShowWildTypeSelectionButton() {
        //     var wildType = GetNodeWildDefinitionType();
        //     if (wildType != null){
        //     	GUILayout.Label(wildType.Name);
        //     }

        //     var wildType = GetNodeWildDefinitionType();
        //     var content = this.GetType();
        //     var args = content.RTGetGenericArguments();
        //     var arg1 = args.FirstOrDefault();
        //     if (arg1 == wildType && arg1 != null){
        //     	EditorUtils.ButtonTypePopup(string.Empty, wildType, (t)=>
        //     	{
        //     		var newType = TryGetNewGenericTypeForWild(wildType, wildType, t, content, null);
        //     		if (newType != null){
        //     			this.ReplaceWith(newType);
        //     		}
        //     	});
        //     }
        // }

        //Draw all ports in order
        void DrawNodePorts() {

            if ( orderedInputs == null || orderedOutputs == null ) {
                return;
            }

            var flowInputsCount = 0;
            var flowOutputsCount = 0;
            var valueInputsCount = 0;
            var valueOutputsCount = 0;

            var anyValuePortDrawn = false;
            var maxIndex = Mathf.Max(orderedInputs.Length, orderedOutputs.Length);
            for ( var i = 0; i < maxIndex; i++ ) {

                Port input = null;
                Port output = null;

                if ( i <= orderedInputs.Length - 1 ) {
                    var port = orderedInputs[i];
                    if ( port.IsFlowPort() ) {
                        flowInputsCount++;
                        input = port;
                    } else {
                        valueInputsCount++;
                        anyValuePortDrawn |= port.willDraw;
                    }
                }

                if ( i <= orderedOutputs.Length - 1 ) {
                    var port = orderedOutputs[i];
                    if ( port.IsFlowPort() || port.IsDelegate() ) {
                        flowOutputsCount++;
                        output = port;
                    } else {
                        valueOutputsCount++;
                        anyValuePortDrawn |= port.willDraw;
                    }
                }

                if ( input != null || output != null ) {
                    GUILayout.BeginHorizontal();
                    if ( input != null ) { DrawNodePort(input); }
                    GUILayout.FlexibleSpace();
                    if ( output != null ) { DrawNodePort(output); }
                    GUILayout.EndHorizontal();
                }
            }

            if ( anyValuePortDrawn ) {
                GUILayout.BeginVertical(Styles.roundedBox);
                maxIndex = Mathf.Max(valueInputsCount, valueOutputsCount);
                for ( var i = 0; i < maxIndex; i++ ) {

                    var indexIn = i + flowInputsCount;
                    var indexOut = i + flowOutputsCount;

                    Port input = null;
                    Port output = null;

                    if ( indexIn <= orderedInputs.Length - 1 ) {
                        input = orderedInputs[indexIn];
                    }

                    if ( indexOut <= orderedOutputs.Length - 1 ) {
                        output = orderedOutputs[indexOut];
                    }

                    if ( input != null && output != null ) {
                        if ( input.name == output.name && input.type == output.type ) {
                            GUILayout.BeginHorizontal();
                            DrawNodePort(input, output);
                            GUILayout.EndHorizontal();
                            continue;
                        }
                    }

                    if ( input != null || output != null ) {
                        GUILayout.BeginHorizontal();
                        if ( input != null ) { DrawNodePort(input); }
                        GUILayout.FlexibleSpace();
                        if ( output != null ) { DrawNodePort(output); }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }

            if ( Event.current.type == EventType.Repaint ) {
                portsHaveDrawnFlag = true;
            }
        }

        //Draw a port
        void DrawNodePort(Port port, Port mirrorOutput = null) {

            if ( port.willDraw || ( mirrorOutput != null && mirrorOutput.willDraw ) ) {

                EditorGUIUtility.SetIconSize(port.bindStatus != Port.BindStatus.Valid ? new Vector2(16, 16) : new Vector2(14, 14));

                port.EnsureCachedGUIContent();
                if ( port.IsInputPort() ) {
                    GUI.color = EditorGUIUtility.isProSkin ? GUI.color : Color.black.WithAlpha(0.6f);
                    GUILayout.Label(port.displayContent.image, Styles.proxyContentImage);
                    GUI.color = Color.white;
                    GUILayout.Label(port.displayContent.text, port.IsFlowPort() ? Styles.leftLabel : Styles.proxyLeftContentLabel);
                    if ( mirrorOutput != null ) { GUILayout.FlexibleSpace(); }
                } else {
                    if ( mirrorOutput != null ) { GUILayout.FlexibleSpace(); }
                    GUILayout.Label(port.displayContent.text, port.IsFlowPort() ? Styles.rightLabel : Styles.proxyRightContentLabel);
                    GUI.color = EditorGUIUtility.isProSkin ? GUI.color : Color.black.WithAlpha(0.6f);
                    GUILayout.Label(port.displayContent.image, Styles.proxyContentImage);
                    GUI.color = Color.white;
                }
                if ( mirrorOutput != null ) {
                    GUI.color = EditorGUIUtility.isProSkin ? GUI.color : Color.black.WithAlpha(0.6f);
                    GUILayout.Label(port.displayContent.image, Styles.proxyContentImage);
                    GUI.color = Color.white;
                }

                EditorGUIUtility.SetIconSize(Vector2.zero);

                if ( Event.current.type == EventType.Repaint ) {
                    var lastRectCenterY = GUILayoutUtility.GetLastRect().center.y;
                    port.posOffsetY = lastRectCenterY;
                    if ( mirrorOutput != null ) {
                        mirrorOutput.posOffsetY = lastRectCenterY;
                    }
                }
            }
        }

        //The inspector panel
        protected override void OnNodeInspectorGUI() {
            if ( this.GetType().RTIsDefined<HasRefreshButtonAttribute>(true) ) {
                if ( GUILayout.Button("Refresh") ) { GatherPorts(); }
                EditorUtils.Separator();
            }

            DrawDefaultInspector();
            EditorUtils.Separator();
            DrawValueInputsGUI();
        }

        //Show the serialized input port values if any
        protected void DrawValueInputsGUI() {
            foreach ( var input in inputPorts.Values.OfType<ValueInput>() ) {
                input.EnsureCachedGUIContent();
                if ( input.isConnected ) {
                    EditorGUILayout.LabelField(input.displayName, "[CONNECTED]");
                    continue;
                }
                var serializationInfo = new InspectedFieldInfo(graph, null, null, null);
                input.serializedValue = EditorUtils.ReflectedFieldInspector(input.displayName, input.serializedValue, input.type, serializationInfo);
            }
        }

        //Override of right click node context menu
        protected override GenericMenu OnContextMenu(GenericMenu menu) {
            menu = base.OnContextMenu(menu);
            if ( outputPorts.Values.Any(p => p is FlowOutput) ) { //breakpoints only work with FlowOutputs
                menu.AddItem(new GUIContent("Breakpoint"), isBreakpoint, () => { isBreakpoint = !isBreakpoint; });
            }

            menu.AddItem(new GUIContent("Verbose Level/Compact (Key 1)"), verboseLevel == VerboseLevel.Compact, () => { verboseLevel = VerboseLevel.Compact; });
            menu.AddItem(new GUIContent("Verbose Level/Partial (Key 2)"), verboseLevel == VerboseLevel.Partial, () => { verboseLevel = VerboseLevel.Partial; });
            menu.AddItem(new GUIContent("Verbose Level/Full (Key 3 or Hold S Key over node)"), verboseLevel == VerboseLevel.Full, () => { verboseLevel = VerboseLevel.Full; });

            var type = this.GetType();
            if ( type.IsGenericType ) {
                menu = EditorUtils.GetPreferedTypesSelectionMenu(type.GetGenericTypeDefinition(), (t) => { this.ReplaceWith(t); }, menu, "Change Generic Type");
            }

            return menu;
        }

#endif
        ///----------------------------------------------------------------------------------------------

    }
}