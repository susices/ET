using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    [Name("Trigger2D")]
    [Category("Events/Object")]
    [Description("Called when 2D Trigger based event happen on target when it has either a Collider2D or a Rigidbody2D")]
    public class Trigger2DEvents_Transform : RouterEventNode<Transform>
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
            router.onTriggerEnter2D += OnTriggerEnter2D;
            router.onTriggerStay2D += OnTriggerStay2D;
            router.onTriggerExit2D += OnTriggerExit2D;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTriggerEnter2D -= OnTriggerEnter2D;
            router.onTriggerStay2D -= OnTriggerStay2D;
            router.onTriggerExit2D -= OnTriggerExit2D;
        }

        void OnTriggerEnter2D(ParadoxNotion.EventData<Collider2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onEnter.Call(new Flow());
        }

        void OnTriggerStay2D(ParadoxNotion.EventData<Collider2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onStay.Call(new Flow());
        }

        void OnTriggerExit2D(ParadoxNotion.EventData<Collider2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onExit.Call(new Flow());
        }
    }

    ///----------------------------------------------------------------------------------------------

    [Name("Trigger2D")]
    [Category("Events/Object")]
    [Description("Called when 2D Trigger based event happen on target")]
    [DoNotList]
    public class Trigger2DEvents : RouterEventNode<Collider2D>
    {

        private FlowOutput onEnter;
        private FlowOutput onStay;
        private FlowOutput onExit;
        private Collider2D receiver;
        private GameObject other;

        protected override void RegisterPorts() {
            onEnter = AddFlowOutput("Enter");
            onStay = AddFlowOutput("Stay");
            onExit = AddFlowOutput("Exit");
            AddValueOutput<Collider2D>("Receiver", () => { return receiver; });
            AddValueOutput<GameObject>("Other", () => { return other; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTriggerEnter2D += OnTriggerEnter2D;
            router.onTriggerStay2D += OnTriggerStay2D;
            router.onTriggerExit2D += OnTriggerExit2D;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTriggerEnter2D -= OnTriggerEnter2D;
            router.onTriggerStay2D -= OnTriggerStay2D;
            router.onTriggerExit2D -= OnTriggerExit2D;
        }

        void OnTriggerEnter2D(ParadoxNotion.EventData<Collider2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onEnter.Call(new Flow());
        }

        void OnTriggerStay2D(ParadoxNotion.EventData<Collider2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onStay.Call(new Flow());
        }

        void OnTriggerExit2D(ParadoxNotion.EventData<Collider2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.other = msg.value.gameObject;
            onExit.Call(new Flow());
        }
    }
}