using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [System.Obsolete("Use 'IReferencedObjectWrapper' along with 'ReferencedObjectTypeAttribute'")]
    abstract public class CustomObjectWrapper : FlowNode
    {
        abstract public void SetTarget(UnityEngine.Object target);
    }

    [System.Obsolete("Use 'IReferencedObjectWrapper' along with 'ReferencedObjectTypeAttribute'")]
    [Icon(runtimeIconTypeCallback: nameof(GetRuntimeIconType))]
    abstract public class CustomObjectWrapper<T> : CustomObjectWrapper where T : UnityEngine.Object
    {
        [SerializeField]
        private T _target;
        public T target {
            get { return _target; }
            set { if ( _target != value ) { _target = value; GatherPorts(); } }
        }

        public override string name { get { return target != null ? target.name : base.name; } }
        public override void SetTarget(UnityEngine.Object target) { if ( target is T ) { this.target = (T)target; } }
        protected System.Type GetRuntimeIconType() { return target != null ? target.GetType() : null; }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI() {
            target = (T)UnityEditor.EditorGUILayout.ObjectField("Target", target, typeof(T), true);
            base.OnNodeInspectorGUI();
        }
#endif
    }
}