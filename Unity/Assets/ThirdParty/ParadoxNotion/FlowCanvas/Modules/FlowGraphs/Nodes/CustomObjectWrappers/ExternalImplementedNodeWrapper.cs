using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [DoNotList]
    [Icon(runtimeIconTypeCallback: nameof(GetRuntimeIconType))]
    public class ExternalImplementedNodeWrapper : FlowNode
    {
        [SerializeField]
        private UnityEngine.Object _target;
        public IExternalImplementedNode target {
            get { return _target as IExternalImplementedNode; }
            set
            {
                if ( !ReferenceEquals(_target, value) ) {
                    _target = value as UnityEngine.Object;
                    GatherPorts();
                }
            }
        }

        public override string name { get { return _target != null ? _target.name : base.name; } }
        protected System.Type GetRuntimeIconType() { return _target?.GetType(); }
        protected override void RegisterPorts() { if ( _target != null ) { target.RegisterPorts(this); } }
        public void SetTarget(IExternalImplementedNode target) { this.target = target; }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI() {
            target = (IExternalImplementedNode)UnityEditor.EditorGUILayout.ObjectField("Target", target as UnityEngine.Object, typeof(IExternalImplementedNode), true);
            base.OnNodeInspectorGUI();
        }
#endif
        ///----------------------------------------------------------------------------------------------
    }
}