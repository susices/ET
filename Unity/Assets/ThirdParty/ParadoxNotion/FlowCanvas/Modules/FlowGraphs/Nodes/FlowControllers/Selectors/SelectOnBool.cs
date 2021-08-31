using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [ExposeAsDefinition]
    [Category("Flow Controllers/Selectors")]
    [Description("Select a Result value out of the two input cases provided, based on a boolean Condition")]
    public class SelectOnBool<T> : FlowControlNode
    {
        ValueInput<bool> condition;
        ValueInput<T> isTrue;
        ValueInput<T> isFalse;
        ValueOutput<T> result;

        protected override void RegisterPorts() {
            condition = AddValueInput<bool>("Condition");
            isTrue = AddValueInput<T>("Is True", "True");
            isFalse = AddValueInput<T>("Is False", "False");
            result = AddValueOutput<T>("Result", "Value", () => { return condition.value ? isTrue.value : isFalse.value; });
        }
    }
}