using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    [Name("Object State")]
    [Category("Events/Object")]
    [Description("OnEnable, OnDisable and OnDestroy callback events for target object")]
    public class ObjectStateEvents : RouterEventNode<Transform>
    {

        private FlowOutput onEnable;
        private FlowOutput onDisable;
        private FlowOutput onDestroy;
        private GameObject receiver;

        protected override void RegisterPorts() {
            onEnable = AddFlowOutput("On Enable");
            onDisable = AddFlowOutput("On Disable");
            onDestroy = AddFlowOutput("On Destroy");
            AddValueOutput<GameObject>("Receiver", () => { return receiver; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onEnable += OnEnable;
            router.onDisable += OnDisable;
            router.onDestroy += OnDestroy;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onEnable -= OnEnable;
            router.onDisable -= OnDisable;
            router.onDestroy -= OnDestroy;
        }

        void OnEnable(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver).gameObject;
            onEnable.Call(new Flow());
        }

        void OnDisable(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver).gameObject;
            onDisable.Call(new Flow());
        }

        void OnDestroy(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver).gameObject;
            onDestroy.Call(new Flow());
        }
    }
}