using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    //We can't use proper versioning here since for that we would need to utilize GetComponent,
    //which is not possible to do in Deserialization. As such we make a completely new version.

    [Name("Collision")]
    [Category("Events/Object")]
    [Description("Called when Collision based events happen on target and expose collision information")]
    public class CollisionEvents_Rigidbody : RouterEventNode<Rigidbody>
    {

        private FlowOutput onEnter;
        private FlowOutput onStay;
        private FlowOutput onExit;
        private Rigidbody receiver;
        private Collision collision;

        protected override void RegisterPorts() {
            onEnter = AddFlowOutput("Enter");
            onStay = AddFlowOutput("Stay");
            onExit = AddFlowOutput("Exit");
            AddValueOutput<Rigidbody>("Receiver", () => { return receiver; });
            AddValueOutput<GameObject>("Other", () => { return collision.gameObject; });
            AddValueOutput<ContactPoint>("Contact Point", () => { return collision.contacts[0]; });
            AddValueOutput<Collision>("Collision Info", () => { return collision; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCollisionEnter += OnCollisionEnter;
            router.onCollisionStay += OnCollisionStay;
            router.onCollisionExit += OnCollisionExit;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCollisionEnter -= OnCollisionEnter;
            router.onCollisionStay -= OnCollisionStay;
            router.onCollisionExit -= OnCollisionExit;
        }

        void OnCollisionEnter(ParadoxNotion.EventData<Collision> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onEnter.Call(new Flow());
        }

        void OnCollisionStay(ParadoxNotion.EventData<Collision> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onStay.Call(new Flow());
        }

        void OnCollisionExit(ParadoxNotion.EventData<Collision> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onExit.Call(new Flow());
        }
    }

    ///----------------------------------------------------------------------------------------------

    [Name("Collision")]
    [Category("Events/Object")]
    [Description("Called when Collision based events happen on target and expose collision information")]
    [DoNotList]
    public class CollisionEvents : RouterEventNode<Collider>
    {

        private FlowOutput onEnter;
        private FlowOutput onStay;
        private FlowOutput onExit;
        private Collider receiver;
        private Collision collision;

        protected override void RegisterPorts() {
            onEnter = AddFlowOutput("Enter");
            onStay = AddFlowOutput("Stay");
            onExit = AddFlowOutput("Exit");
            AddValueOutput<Collider>("Receiver", () => { return receiver; });
            AddValueOutput<GameObject>("Other", () => { return collision.gameObject; });
            AddValueOutput<ContactPoint>("Contact Point", () => { return collision.contacts[0]; });
            AddValueOutput<Collision>("Collision Info", () => { return collision; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCollisionEnter += OnCollisionEnter;
            router.onCollisionStay += OnCollisionStay;
            router.onCollisionExit += OnCollisionExit;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onCollisionEnter -= OnCollisionEnter;
            router.onCollisionStay -= OnCollisionStay;
            router.onCollisionExit -= OnCollisionExit;
        }

        void OnCollisionEnter(ParadoxNotion.EventData<Collision> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onEnter.Call(new Flow());
        }

        void OnCollisionStay(ParadoxNotion.EventData<Collision> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onStay.Call(new Flow());
        }

        void OnCollisionExit(ParadoxNotion.EventData<Collision> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.collision = msg.value;
            onExit.Call(new Flow());
        }
    }
}