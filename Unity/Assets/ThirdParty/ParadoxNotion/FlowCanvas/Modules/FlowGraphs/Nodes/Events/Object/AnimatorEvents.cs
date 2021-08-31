using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    [Name("Animator")]
    [Category("Events/Object")]
    [Description("Calls Animator based events. Note that using this node will override root motion as usual, but you can call 'Apply Builtin Root Motion' to get it back.")]
    public class AnimatorEvents : RouterEventNode<Animator>
    {

        private FlowOutput onAnimatorMove;
        private FlowOutput onAnimatorIK;
        private Animator receiver;
        private int layerIndex;

        protected override void RegisterPorts() {
            onAnimatorMove = AddFlowOutput("On Animator Move");
            onAnimatorIK = AddFlowOutput("On Animator IK");
            AddValueOutput<Animator>("Receiver", () => { return this.receiver; });
            AddValueOutput<int>("Layer Index", () => { return layerIndex; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onAnimatorMove += OnAnimatorMove;
            router.onAnimatorIK += OnAnimatorIK;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onAnimatorMove -= OnAnimatorMove;
            router.onAnimatorIK -= OnAnimatorIK;
        }

        void OnAnimatorMove(ParadoxNotion.EventData msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            onAnimatorMove.Call(new Flow());
        }

        void OnAnimatorIK(ParadoxNotion.EventData<int> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.layerIndex = msg.value;
            onAnimatorIK.Call(new Flow());
        }
    }
}