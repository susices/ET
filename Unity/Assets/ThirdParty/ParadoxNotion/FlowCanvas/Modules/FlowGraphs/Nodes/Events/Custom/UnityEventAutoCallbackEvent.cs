using ParadoxNotion;
using ParadoxNotion.Design;
using System.Reflection;
using UnityEngine.Events;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using NodeCanvas.Framework;

namespace FlowCanvas.Nodes
{

    [DoNotList]
    [Name("Unity Event")]
    [Description("Automatically Subscribes to the target UnityEvent when the graph is enabled, and is called when the event is raised")]
    public class UnityEventAutoCallbackEvent : EventNode, IReflectedWrapper
    {

        [fsSerializeAs("_field")]
        private SerializedUnityEventInfo _eventMember;

        private ReflectedUnityEvent reflectedEvent;
        private UnityEventBase unityEvent;
        private ValueInput instancePort;
        private FlowOutput callback;
        private object[] args;

        private MemberInfo member => _eventMember != null ? _eventMember.AsMemberInfo() : null;
        private bool isStatic => _eventMember != null ? _eventMember.isStatic : false;
        private System.Type eventType => _eventMember != null ? _eventMember.memberType : null;
        private FieldInfo field => _eventMember;
        private PropertyInfo prop => _eventMember;


        public override string name {
            get
            {
                if ( _eventMember == null ) { return "NOT SET"; }
                if ( member == null ) { return _eventMember.AsString().FormatError(); }
                if ( isStatic ) {
                    return string.Format("{0} ({1})", base.name, member.RTReflectedOrDeclaredType().FriendlyName());
                }
                return base.name;
            }
        }

        ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo() { return _eventMember; }

        public void SetEvent(MemberInfo newMember, object instance = null) {
            _eventMember = new SerializedUnityEventInfo(newMember);
            GatherPorts();
        }

        protected override void RegisterPorts() {
            if ( member == null ) {
                return;
            }

            if ( reflectedEvent == null ) {
                reflectedEvent = new ReflectedUnityEvent(eventType);
            }

            if ( !isStatic ) {
                instancePort = AddValueInput(member.RTReflectedOrDeclaredType().FriendlyName(), member.RTReflectedOrDeclaredType(), "Instance");
            }

            args = new object[reflectedEvent.parameters.Length];
            for ( var _i = 0; _i < reflectedEvent.parameters.Length; _i++ ) {
                var i = _i;
                var parameter = reflectedEvent.parameters[i];
                AddValueOutput(parameter.Name, "arg" + i, parameter.ParameterType, () => { return args[i]; });
            }

            callback = AddFlowOutput(member.Name, "Event");
        }

        public override void OnGraphStarted() {
            if ( member == null ) {
                return;
            }

            object instance = null;
            if ( !isStatic ) {
                instance = instancePort.value;
                if ( instance == null ) {
                    Fail("Target is null");
                    return;
                }
            }

            if ( field != null ) { unityEvent = (UnityEventBase)field.GetValue(instance); }
            if ( prop != null ) { unityEvent = (UnityEventBase)prop.GetValue(instance); }

            if ( unityEvent != null ) {
                reflectedEvent.StartListening(unityEvent, OnEventRaised);
            }
        }

        public override void OnGraphStoped() {
            if ( unityEvent != null ) {
                reflectedEvent.StopListening(unityEvent, OnEventRaised);
            }
        }

        void OnEventRaised(params object[] args) {
            this.args = args;
            callback.Call(new Flow());
        }

    }
}