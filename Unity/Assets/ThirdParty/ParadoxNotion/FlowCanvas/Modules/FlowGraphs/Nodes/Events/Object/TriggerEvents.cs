using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    [Name("Trigger")]
    [Category("Events/Object")]
    [Description("Called when Trigger based event happen on target when it has either a Collider or a Rigidbody")]
    public class TriggerEvents_Transform : RouterEventNode<Transform>
    {

        private FlowOutput onEnter;
        private FlowOutput onStay;
        private FlowOutput onExit;
        private Transform receiver;
        private GameObject other;

        protected override void RegisterPorts() {
            onEnter = AddFlowOutput("Enter");
            onStay = AddFlowOutput("Stay");
            onExit = AddFlowOutput("Exit");
            AddValueOutput<Transform>("Receiver", () => { return receiver; });
            AddValueOutput<GameObject>("Other", () => { return other; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTriggerEnter += OnTriggerEnter;
            router.onTriggerStay += OnTriggerStay;
            router.onTriggerExit += OnTriggerExit;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTriggerEnter -= OnTriggerEnter;
            router.onTriggerStay -= OnTriggerStay;
            router.onTriggerExit -= OnTriggerExit;
        }

        void OnTriggerEnter(ParadoxNotion.EventData<Collider> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onEnter.Call(new Flow());
        }

        void OnTriggerStay(ParadoxNotion.EventData<Collider> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onStay.Call(new Flow());
        }

        void OnTriggerExit(ParadoxNotion.EventData<Collider> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onExit.Call(new Flow());
        }
    }

    ///----------------------------------------------------------------------------------------------

    [Name("Trigger")]
    [Category("Events/Object")]
    [Description("Called when Trigger based event happen on target")]
    [DoNotList]
    public class TriggerEvents : RouterEventNode<Collider>
    {

        private FlowOutput onEnter;
        private FlowOutput onStay;
        private FlowOutput onExit;
        private Collider receiver;
        private GameObject other;

        protected override void RegisterPorts() {
            onEnter = AddFlowOutput("Enter");
            onStay = AddFlowOutput("Stay");
            onExit = AddFlowOutput("Exit");
            AddValueOutput<Collider>("Receiver", () => { return receiver; });
            AddValueOutput<GameObject>("Other", () => { return other; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTriggerEnter += OnTriggerEnter;
            router.onTriggerStay += OnTriggerStay;
            router.onTriggerExit += OnTriggerExit;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTriggerEnter -= OnTriggerEnter;
            router.onTriggerStay -= OnTriggerStay;
            router.onTriggerExit -= OnTriggerExit;
        }

        void OnTriggerEnter(ParadoxNotion.EventData<Collider> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onEnter.Call(new Flow());
        }

        void OnTriggerStay(ParadoxNotion.EventData<Collider> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onStay.Call(new Flow());
        }

        void OnTriggerExit(ParadoxNotion.EventData<Collider> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onExit.Call(new Flow());
        }
    }
}