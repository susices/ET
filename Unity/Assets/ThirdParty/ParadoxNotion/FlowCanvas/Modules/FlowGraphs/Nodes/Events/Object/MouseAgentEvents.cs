using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    [Name("Mouse")]
    [Category("Events/Object")]
    [Description("Called when mouse based operations happen on target collider")]
    public class MouseAgentEvents : RouterEventNode<Collider>
    {

        private FlowOutput onEnter;
        private FlowOutput onOver;
        private FlowOutput onExit;
        private FlowOutput onDown;
        private FlowOutput onUp;
        private FlowOutput onDrag;

        private Collider receiver;
        private RaycastHit hit;

        protected override void RegisterPorts() {
            onDown = AddFlowOutput("Down");
            onUp = AddFlowOutput("Up");
            onEnter = AddFlowOutput("Enter");
            onOver = AddFlowOutput("Over");
            onExit = AddFlowOutput("Exit");
            onDrag = AddFlowOutput("Drag");
            AddValueOutput<Collider>("Receiver", () => { return receiver; });
            AddValueOutput<RaycastHit>("Info", () => { return hit; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onMouseEnter += OnMouseEnter;
            router.onMouseOver += OnMouseOver;
            router.onMouseExit += OnMouseExit;
            router.onMouseDown += OnMouseDown;
            router.onMouseUp += OnMouseUp;
            router.onMouseDrag += OnMouseDrag;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onMouseEnter -= OnMouseEnter;
            router.onMouseOver -= OnMouseOver;
            router.onMouseExit -= OnMouseExit;
            router.onMouseDown -= OnMouseDown;
            router.onMouseUp -= OnMouseUp;
            router.onMouseDrag -= OnMouseDrag;
        }

        void OnMouseEnter(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            StoreHit();
            onEnter.Call(new Flow());
        }

        void OnMouseOver(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            StoreHit();
            onOver.Call(new Flow());
        }

        void OnMouseExit(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            StoreHit();
            onExit.Call(new Flow());
        }

        void OnMouseDown(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            StoreHit();
            onDown.Call(new Flow());
        }

        void OnMouseUp(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            StoreHit();
            onUp.Call(new Flow());
        }

        void OnMouseDrag(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            StoreHit();
            onDrag.Call(new Flow());
        }

        void StoreHit() {
            if ( Camera.main != null ) {
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
            }
        }
    }
}