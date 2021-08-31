using ParadoxNotion.Design;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    [Category("Events/Object")]
    [Description("Events relevant to transform changes")]
    public class TransformEvents : RouterEventNode<Transform>
    {

        private FlowOutput onParentChanged;
        private FlowOutput onChildrenChanged;
        private Transform receiver;
        private Transform parent;
        private IEnumerable<Transform> children;

        protected override void RegisterPorts() {
            onParentChanged = AddFlowOutput("On Transform Parent Changed");
            onChildrenChanged = AddFlowOutput("On Transform Children Changed");
            AddValueOutput<Transform>("Receiver", () => { return receiver; });
            AddValueOutput<Transform>("Parent", () => { return parent; });
            AddValueOutput<IEnumerable<Transform>>("Children", () => { return children; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTransformParentChanged += OnTransformParentChanged;
            router.onTransformChildrenChanged += OnTransformChildrenChanged;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onTransformParentChanged -= OnTransformParentChanged;
            router.onTransformChildrenChanged -= OnTransformChildrenChanged;
        }

        void OnTransformParentChanged(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.parent = receiver.parent;
            this.children = receiver.Cast<Transform>();
            onParentChanged.Call(new Flow());
        }

        void OnTransformChildrenChanged(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.parent = receiver.parent;
            this.children = receiver.Cast<Transform>();
            onChildrenChanged.Call(new Flow());
        }
    }
}