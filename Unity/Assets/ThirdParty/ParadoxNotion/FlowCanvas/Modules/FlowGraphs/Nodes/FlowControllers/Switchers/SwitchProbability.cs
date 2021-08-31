using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [Category("Flow Controllers/Switchers")]
    [Description("Calls one output based on it's probability to be selected, in comparison to all other outputs. The probability of each output is respectively set by the same name float input.\nFor example, if all input values are the same (eg '1'), then all outputs have the same chance to be called.")]
    [ContextDefinedInputs(typeof(Flow), typeof(float))]
    public class SwitchProbability : FlowControlNode
    {
        [SerializeField]
        [ExposeField]
        [GatherPortsCallback]
        [MinValue(2)]
        [DelayedField]
        private int _portCount = 2;

        private List<ValueInput<float>> probabilityValues;
        private List<FlowOutput> probabilityOuts;
        private float[] cacheValues;

        private int current;

        protected override void RegisterPorts() {
            probabilityValues = new List<ValueInput<float>>();
            probabilityOuts = new List<FlowOutput>();
            cacheValues = new float[_portCount];
            for ( var i = 0; i < _portCount; i++ ) {
                probabilityValues.Add(AddValueInput<float>(StringUtils.GetAlphabetLetter(i), i.ToString()).SetDefaultAndSerializedValue(1));
                probabilityOuts.Add(AddFlowOutput(StringUtils.GetAlphabetLetter(i), i.ToString()));
            }

            AddValueOutput<int>("Current", () => { return current; });
            AddFlowInput("In", Enter);
        }

        void Enter(Flow f) {
            var total = 0f;
            for ( var i = 0; i < _portCount; i++ ) {
                cacheValues[i] = probabilityValues[i].value;
                total += cacheValues[i];
            }
            var probability = UnityEngine.Random.Range(0f, total);

            for ( var i = 0; i < _portCount; i++ ) {
                if ( probability > cacheValues[i] ) {
                    probability -= cacheValues[i];
                    continue;
                }

                current = i;
                probabilityOuts[i].Call(f);
                break;
            }
        }
    }
}