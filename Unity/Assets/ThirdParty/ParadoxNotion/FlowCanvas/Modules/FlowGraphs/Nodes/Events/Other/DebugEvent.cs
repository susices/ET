using ParadoxNotion.Design;
using UnityEngine;
using System.Diagnostics;
using NodeCanvas.Framework;
using Logger = ParadoxNotion.Services.Logger;

namespace FlowCanvas.Nodes
{

    [Description("Use to debug send a Flow Signal in PlayMode Only")]
    [Category("Events/Other")]
    public class DebugEvent : EventNode

#if UNITY_EDITOR
    , IUpdatable
#endif

    {

#if UNITY_EDITOR
        private FlowOutput o;
        private bool send;

        protected override void RegisterPorts() {
            o = AddFlowOutput("Out");
        }

        void IUpdatable.Update() {
            if ( send ) {
                send = false;
                var sw = new Stopwatch();
                sw.Start();
                o.Call(new Flow());
                sw.Stop();
                Logger.Log(string.Format("Debug Event Elapsed Time: {0} ms.", sw.ElapsedMilliseconds), LogTag.EDITOR, this);
            }
        }

        ///----------------------------------------------------------------------------------------------
        protected override void OnNodeInspectorGUI() {
            if ( GUILayout.Button("Call") ) {
                if ( Application.isPlaying ) {
                    //we do this only for the debugging to show, cause it doesn if we fire the port in here (OnGUI) but it works fine otherwise
                    send = true;
                } else {
                    Logger.LogWarning("Debug Flow Signal Event will only work in PlayMode", LogTag.EDITOR, this);
                }
            }
        }

#else

    protected override void RegisterPorts(){}
    
#endif


    }
}