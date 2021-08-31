using ParadoxNotion.Design;
using UnityEngine;
using NodeCanvas.Framework;

namespace FlowCanvas.Nodes
{

    [Name("Character Controller")]
    [Category("Events/Object")]
    [Description("Called when the Character Controller hits a collider while performing a Move")]
    public class CharacterControllerEvents : RouterEventNode<CharacterController>, IUpdatable
    {

        private FlowOutput onHit;
        private CharacterController receiver;
        private ControllerColliderHit hitInfo;

        private FlowOutput onGrounded;
        private FlowOutput onUnGrounded;

        private bool wasGrounded;
        private bool[] wasGroundedMulti;

        public override void OnPostGraphStarted() {
            base.OnPostGraphStarted();
            if ( base.targetMode == TargetMode.SingleTarget ) {
                wasGrounded = base.target.value.isGrounded;
            }
            if ( base.targetMode == TargetMode.MultipleTargets ) {
                wasGroundedMulti = new bool[base.targets.value.Count];
            }
        }

        void IUpdatable.Update() {
            if ( base.targetMode == TargetMode.SingleTarget ) {
                var isGrounded = base.target.value.isGrounded;
                if ( isGrounded && !wasGrounded ) { onGrounded.Call(Flow.New); }
                if ( !isGrounded && wasGrounded ) { onUnGrounded.Call(Flow.New); }
                wasGrounded = isGrounded;
            }
            if ( base.targetMode == TargetMode.MultipleTargets ) {
                var list = targets.value;
                for ( var i = 0; i < list.Count; i++ ) {
                    var t = list[i];
                    if ( t == null ) { continue; }
                    var isGrounded = t.isGrounded;
                    if ( isGrounded && !wasGroundedMulti[i] ) {
                        this.receiver = t;
                        onGrounded.Call(Flow.New);
                    }
                    if ( !isGrounded && wasGroundedMulti[i] ) {
                        this.receiver = t;
                        onUnGrounded.Call(Flow.New);
                    }
                    wasGroundedMulti[i] = isGrounded;
                }
            }
        }

        protected override void RegisterPorts() {
            onHit = AddFlowOutput("Collider Hit");
            onGrounded = AddFlowOutput("On Grounded");
            onUnGrounded = AddFlowOutput("On UnGrounded");

            AddValueOutput<CharacterController>("Receiver", () => { return receiver; });
            AddValueOutput<GameObject>("Other", () => { return hitInfo.gameObject; });
            AddValueOutput<Vector3>("Collision Point", () => { return hitInfo.point; });
            AddValueOutput<ControllerColliderHit>("Collision Info", () => { return hitInfo; });
        }

        protected override void Subscribe(ParadoxNotion.Services.EventRouter router) {
            router.onControllerColliderHit += OnControllerColliderHit;
        }

        protected override void UnSubscribe(ParadoxNotion.Services.EventRouter router) {
            router.onControllerColliderHit -= OnControllerColliderHit;
        }

        void OnControllerColliderHit(ParadoxNotion.EventData<ControllerColliderHit> msg) {
            this.receiver = ResolveReceiver(msg.receiver);
            this.hitInfo = msg.value;
            onHit.Call(new Flow());
        }
    }
}