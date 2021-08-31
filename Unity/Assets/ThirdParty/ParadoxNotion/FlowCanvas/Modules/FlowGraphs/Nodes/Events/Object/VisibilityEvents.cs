using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    [Name("Visibility")]
    [Category("Events/Object")]
    [Description("Calls events based on object's render visibility")]
    public class VisibilityEvents : RouterEventNode<Transform>
    {

        private FlowOutput onVisible;
        private FlowOutput onInvisible;
        private GameObject receiver;

        protected override void RegisterPorts() {
            onVisible = AddFlowOutput("Became Visible");
            onInvisible = AddFlowOutput("Became Invisible");
            AddValueOutput<GameObject>("Receiver", () => { return receiver; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onBecameVisible += OnBecameVisible;
            router.onBecameInvisible += OnBecameInvisible;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onBecameVisible -= OnBecameVisible;
            router.onBecameInvisible -= OnBecameInvisible;
        }

        void OnBecameVisible(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver).gameObject;
            onVisible.Call(new Flow());
        }

        void OnBecameInvisible(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver).gameObject;
            onInvisible.Call(new Flow());
        }
    }
}