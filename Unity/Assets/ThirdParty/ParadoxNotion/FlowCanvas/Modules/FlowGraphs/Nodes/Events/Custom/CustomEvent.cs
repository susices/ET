using ParadoxNotion.Design;
using NodeCanvas.Framework;
using Logger = ParadoxNotion.Services.Logger;

namespace FlowCanvas.Nodes
{

    [Name("Custom Event", 100)]
    [Description("Called when a custom event is received on target(s).\n- Receiver, is the object which received the event.\n- Sender, is the object which invoked the event.\n\n- To send an event from a graph use the SendEvent node.\n- To send an event from code use: 'FlowScriptController.SendEvent(string)'")]
    [Category("Events/Custom")]
    public class CustomEvent : RouterEventNode<GraphOwner>
    {

        [RequiredField, DelayedField]
        public BBParameter<string> eventName = "EventName";

        private FlowOutput onReceived;
        private GraphOwner sender;
        private GraphOwner receiver;

        public override string name {
            get { return base.name + string.Format(" [ <color=#DDDDDD>{0}</color> ]", eventName); }
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCustomEvent += OnCustomEvent;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCustomEvent -= OnCustomEvent;
        }

        protected override void RegisterPorts() {
            onReceived = AddFlowOutput("Received");
            AddValueOutput<GraphOwner>("Receiver", () => { return receiver; });
            AddValueOutput<GraphOwner>("Sender", () => { return sender; });
        }

        void OnCustomEvent(string eventName, ParadoxNotion.IEventData msg) {
            if ( eventName.Equals(this.eventName.value, System.StringComparison.OrdinalIgnoreCase) ) {
                var senderGraph = Graph.GetElementGraph(msg.sender);
                this.sender = senderGraph != null ? senderGraph.agent as GraphOwner : null;
                this.receiver = ResolveReceiver(msg.receiver);
                Logger.Log(string.Format("Event Received from ({0}): '{1}'", receiver.name, eventName), LogTag.EVENT, this);
                onReceived.Call(new Flow());
            }
        }
    }

    ///----------------------------------------------------------------------------------------------

    [Name("Custom Event", 100)]
    [Description("Called when a custom event is received on target(s).\n- Receiver, is the object which received the event.\n- Sender, is the object which invoked the event.\n\n- To send an event from a graph use the SendEvent(T) node.\n- To send an event from code use: 'FlowScriptController.SendEvent(string, T)'")]
    [Category("Events/Custom")]
    [ContextDefinedOutputs(typeof(Wild))]
    public class CustomEvent<T> : RouterEventNode<GraphOwner>
    {

        [RequiredField, DelayedField]
        public BBParameter<string> eventName = "EventName";

        private FlowOutput onReceived;
        private GraphOwner sender;
        private GraphOwner receiver;
        private T receivedValue;

        public override string name {
            get { return base.name + string.Format(" [ <color=#DDDDDD>{0}</color> ]", eventName); }
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCustomEvent += OnCustomEvent;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCustomEvent -= OnCustomEvent;
        }

        protected override void RegisterPorts() {
            onReceived = AddFlowOutput("Received");
            AddValueOutput<GraphOwner>("Receiver", () => { return receiver; });
            AddValueOutput<GraphOwner>("Sender", () => { return sender; });
            AddValueOutput<T>("Event Value", () => { return receivedValue; });
        }

        void OnCustomEvent(string eventName, ParadoxNotion.IEventData msg) {
            if ( eventName.Equals(this.eventName.value, System.StringComparison.OrdinalIgnoreCase) ) {
                var senderGraph = Graph.GetElementGraph(msg.sender);
                this.sender = senderGraph != null ? senderGraph.agent as GraphOwner : null;
                this.receiver = ResolveReceiver(msg.receiver);

                if ( msg is ParadoxNotion.EventData<T> ) {
                    receivedValue = ( (ParadoxNotion.EventData<T>)msg ).value;
                } else if ( msg.valueBoxed is T ) { receivedValue = (T)msg.valueBoxed; }

                Logger.Log(string.Format("Event Received from ({0}): '{1}'", receiver.name, eventName), LogTag.EVENT, this);
                onReceived.Call(new Flow());
            }
        }
    }

}