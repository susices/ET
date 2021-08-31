using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [Category("Flow Controllers/Flow Merge")]
    [Description("Await all flow inputs to be called within the max allowed await time.\n- If MaxAwaitTime is 0, then all inputs must be called at the same time.\n- If MaxAwaitTime is -1, then the await time is infinity.")]
    [System.Obsolete("Use AND")]
    public class AwaitAll : FlowControlNode
    {

        [SerializeField, ExposeField]
        [MinValue(2), DelayedField]
        [GatherPortsCallback]
        private int _portCount = 2;

        public float maxAwaitTime = -1;

        private FlowOutput fOut;
        private float[] calls;
        private int lastFrameCall;

        public override void OnGraphStarted() { Reset(); }

        protected override void RegisterPorts() {
            calls = new float[_portCount];
            fOut = AddFlowOutput("Out");
            for ( var _i = 0; _i < _portCount; _i++ ) {
                var i = _i;
                AddFlowInput(i.ToString(), (f) => { Check(i, f); });
            }
        }

        void Reset() {
            for ( var i = 0; i < calls.Length; i++ ) {
                calls[i] = float.NegativeInfinity;
            }
        }

        void Check(int index, Flow f) {
            var t = maxAwaitTime < 0 ? 0 : Time.time;
            calls[index] = t;
            for ( var i = 0; i < calls.Length; i++ ) {
                if ( t - calls[i] > Mathf.Abs(maxAwaitTime) ) { return; }
            }

            if ( Time.frameCount != lastFrameCall ) {
                lastFrameCall = Time.frameCount;
                Reset();
                fOut.Call(f);
            }
        }



        // protected override void OnNodeGUI() {
        //     base.OnNodeGUI();
        //     for ( var i = 0; i < calls.Length; i++ ) {
        //         GUILayout.Label($"{i.ToString()} -> {calls[i].ToString("0.00")}");
        //     }
        // }
    }
}