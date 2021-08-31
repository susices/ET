using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    //We can't use proper versioning here since for that we would need to utilize GetComponent,
    //which is not possible to do in Deserialization. As such we make a completely new version.

    [Name("Collision2D")]
    [Category("Events/Object")]
    [Description("Called when 2D Collision based events happen on target and expose collision information")]
    public class Collision2DEvents_Rigidbody : RouterEventNode<Rigidbody2D>
    {

        private FlowOutput onEnter;
        private FlowOutput onStay;
        private FlowOutput onExit;
        private Rigidbody2D receiver;
        private Collision2D collision;

        protected override void RegisterPorts() {
            onEnter = AddFlowOutput("Enter");
            onStay = AddFlowOutput("Stay");
            onExit = AddFlowOutput("Exit");
            AddValueOutput<Rigidbody2D>("Receiver", () => { return receiver; });
            AddValueOutput<GameObject>("Other", () => { return collision.gameObject; });
            AddValueOutput<ContactPoint2D>("Contact Point", () => { return collision.contacts[0]; });
            AddValueOutput<Collision2D>("Collision Info", () => { return collision; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCollisionEnter2D += OnCollisionEnter2D;
            router.onCollisionStay2D += OnCollisionStay2D;
            router.onCollisionExit2D += OnCollisionExit2D;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCollisionEnter2D -= OnCollisionEnter2D;
            router.onCollisionStay2D -= OnCollisionStay2D;
            router.onCollisionExit2D -= OnCollisionExit2D;
        }

        void OnCollisionEnter2D(ParadoxNotion.EventData<Collision2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onEnter.Call(new Flow());
        }

        void OnCollisionStay2D(ParadoxNotion.EventData<Collision2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onStay.Call(new Flow());
        }

        void OnCollisionExit2D(ParadoxNotion.EventData<Collision2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onExit.Call(new Flow());
        }
    }

    ///----------------------------------------------------------------------------------------------

    [Name("Collision2D")]
    [Category("Events/Object")]
    [Description("Called when 2D Collision based events happen on target and expose collision information")]
    [DoNotList]
    public class Collision2DEvents : RouterEventNode<Collider2D>
    {

        private FlowOutput onEnter;
        private FlowOutput onStay;
        private FlowOutput onExit;
        private Collider2D receiver;
        private Collision2D collision;

        protected override void RegisterPorts() {
            onEnter = AddFlowOutput("Enter");
            onStay = AddFlowOutput("Stay");
            onExit = AddFlowOutput("Exit");
            AddValueOutput<Collider2D>("Receiver", () => { return receiver; });
            AddValueOutput<GameObject>("Other", () => { return collision.gameObject; });
            AddValueOutput<ContactPoint2D>("Contact Point", () => { return collision.contacts[0]; });
            AddValueOutput<Collision2D>("Collision Info", () => { return collision; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCollisionEnter2D += OnCollisionEnter2D;
            router.onCollisionStay2D += OnCollisionStay2D;
            router.onCollisionExit2D += OnCollisionExit2D;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCollisionEnter2D -= OnCollisionEnter2D;
            router.onCollisionStay2D -= OnCollisionStay2D;
            router.onCollisionExit2D -= OnCollisionExit2D;
        }

        void OnCollisionEnter2D(ParadoxNotion.EventData<Collision2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onEnter.Call(new Flow());
        }

        void OnCollisionStay2D(ParadoxNotion.EventData<Collision2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onStay.Call(new Flow());
        }

        void OnCollisionExit2D(ParadoxNotion.EventData<Collision2D> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onExit.Call(new Flow());
        }
    }
}