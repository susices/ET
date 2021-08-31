#define DO_EDITOR_BINDING //comment this out to test the real performance without editor binding specifics

using UnityEngine;
using ParadoxNotion;

namespace FlowCanvas
{

    ///Value bindings use the generic version of FlowBinderConnection.
    ///T is always the same at the 'target' ValueInput type.
    public class BinderConnection<T> : BinderConnection
    {

        ///Binds source and target value ports
        public override void Bind() {

            if ( !isActive ) {
                return;
            }

            ( targetPort as ValueInput<T> ).BindTo((ValueOutput)sourcePort);

#if UNITY_EDITOR && DO_EDITOR_BINDING
            ( targetPort as ValueInput<T> ).Append(GetValue);
#else
            sourcePort.connections++;
            targetPort.connections++;
#endif
        }

        ///Unbinds source and target value ports
        public override void UnBind() {
            if ( targetPort is ValueInput ) {
                ( targetPort as ValueInput ).UnBind();
            }
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        private T transferValue;
        private bool hasExecute;

        protected override string GetTransferDataLabel() {
            if ( Application.isPlaying && hasExecute ) {
                return transferValue.ToStringAdvanced().CapLength(25);
            }
            return null;
        }

        protected override void OnConnectionInspectorGUI() {
            base.OnConnectionInspectorGUI();
            if ( sourcePort == null || targetPort == null ) { return; }
            if ( !targetPort.type.IsAssignableFrom(sourcePort.type) ) {
                GUILayout.Label(string.Format("AutoConvert: {0} ➲ {1}", sourcePort.type.FriendlyName(), targetPort.type.FriendlyName()));
            }
        }

        //in editor binding this is appended to get the value, display it and blink the binder
        void GetValue(T result) {
            hasExecute = true;
            transferValue = result;
            base.BlinkStatus();
        }

#endif
        ///----------------------------------------------------------------------------------------------

    }
}