using UnityEngine;
using ParadoxNotion;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using System.Reflection;
using System;

namespace FlowCanvas.Nodes
{

    ///----------------------------------------------------------------------------------------------
    class ReflectedFieldNodeWrapper_0
    {
        [SerializeField] public string fieldName = null;
        [SerializeField] public Type targetType = null;
    }
    ///----------------------------------------------------------------------------------------------


    [DoNotList]
    [Icon(runtimeIconTypeCallback: nameof(GetRuntimeIconType))]
    [fsMigrateVersions(typeof(ReflectedFieldNodeWrapper_0))]
    public class ReflectedFieldNodeWrapper : FlowNode, IReflectedWrapper, IMigratable<ReflectedFieldNodeWrapper_0>
    {

        ///----------------------------------------------------------------------------------------------
        void IMigratable<ReflectedFieldNodeWrapper_0>.Migrate(ReflectedFieldNodeWrapper_0 model) {
            this._field = new SerializedFieldInfo(model.targetType?.RTGetField(model.fieldName));
        }
        ///----------------------------------------------------------------------------------------------


        ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo() { return _field; }
        System.Type GetRuntimeIconType() { return field != null ? field.DeclaringType : null; }

        public enum AccessMode
        {
            GetField,
            SetField
        }

        [SerializeField]
        private SerializedFieldInfo _field;
        [SerializeField]
        private AccessMode accessMode;

        private BaseReflectedFieldNode reflectedFieldNode { get; set; }

        private FieldInfo field => _field;

        public override string name {
            get
            {
                if ( field != null ) {
                    var isGet = accessMode == AccessMode.GetField;
                    var isStatic = field.IsStatic;
                    var isConstant = field.IsConstant();
                    if ( isConstant ) {
                        return string.Format("{0}.{1}", field.DeclaringType.FriendlyName(), field.Name);
                    }
                    if ( isStatic ) {
                        return string.Format("{0} {1}.{2}", ( isGet ? "Get" : "Set" ), field.DeclaringType.FriendlyName(), field.Name);
                    }
                    return string.Format("{0} {1}", ( isGet ? "Get" : "Set" ), field.Name);
                }
                return _field.AsString().FormatError();
            }
        }

#if UNITY_EDITOR
        public override string description {
            get { return field != null ? DocsByReflection.GetMemberSummary(field) : "Missing Field"; }
        }
#endif

        public void SetField(FieldInfo newField, AccessMode mode, object instance = null) {

            if ( newField == null ) {
                return;
            }

            newField = newField.GetBaseDefinition();
            _field = new SerializedFieldInfo(newField);
            accessMode = mode;
            GatherPorts();

            if ( instance != null && !newField.IsStatic ) {
                var port = (ValueInput)GetFirstInputOfType(instance.GetType());
                if ( port != null ) {
                    port.serializedValue = instance;
                }
            }
        }

        //new reflection
        protected override void RegisterPorts() {

            if ( field == null ) {
                return;
            }

            reflectedFieldNode = BaseReflectedFieldNode.GetFieldNode(field);
            if ( reflectedFieldNode != null ) {
                reflectedFieldNode.RegisterPorts(this, accessMode);
            }
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        protected override void OnNodeInspectorGUI() {
            if ( field != null && !field.IsReadOnly() ) {
                var newMode = (AccessMode)UnityEditor.EditorGUILayout.EnumPopup(accessMode);
                if ( accessMode != newMode ) { SetField(field, newMode); }
            }
            base.OnNodeInspectorGUI();
        }
#endif
        ///----------------------------------------------------------------------------------------------

    }
}