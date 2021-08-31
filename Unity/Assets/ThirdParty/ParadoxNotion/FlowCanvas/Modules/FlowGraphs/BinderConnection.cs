#define DO_EDITOR_BINDING //comment this out to test the real performance without editor binding specifics

using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;
using Logger = ParadoxNotion.Services.Logger;

namespace FlowCanvas
{

    public class BinderConnection : Connection
    {

        [ParadoxNotion.Serialization.FullSerializer.fsSerializeAs("_sourcePortName")]
        private string _sourcePortID;
        [ParadoxNotion.Serialization.FullSerializer.fsSerializeAs("_targetPortName")]
        private string _targetPortID;

        [System.NonSerialized]
        private Port _sourcePort;
        [System.NonSerialized]
        private Port _targetPort;

        ///The source port ID name this binder is connected to
        public string sourcePortID {
            get { return sourcePort != null ? sourcePort.ID : _sourcePortID; }
            private set { _sourcePortID = value; }
        }

        ///The target port ID name this binder is connected to
        public string targetPortID {
            get { return targetPort != null ? targetPort.ID : _targetPortID; }
            private set { _targetPortID = value; }
        }

        ///The source Port
        public Port sourcePort {
            get
            {
                if ( _sourcePort == null ) {
                    if ( sourceNode is FlowNode ) { //In case it's 'MissingNode'
                        _sourcePort = ( sourceNode as FlowNode ).GetOutputPort(_sourcePortID);
                    }
                }
                return _sourcePort;
            }
        }

        ///The target Port
        public Port targetPort {
            get
            {
                if ( _targetPort == null ) {
                    if ( targetNode is FlowNode ) { //In case it's 'MissingNode'
                        _targetPort = ( targetNode as FlowNode ).GetInputPort(_targetPortID);
                    }
                }
                return _targetPort;
            }
        }

        ///The binder type. In case of Value connection, BinderConnection<T> is used, else it's basicaly a Flow binding
        public System.Type bindingType {
            get { return GetType().RTIsGenericType() ? GetType().RTGetGenericArguments()[0] : typeof(Flow); }
        }

        ///----------------------------------------------------------------------------------------------

        ///Create a NEW BinderConnection object between two ports
        public static BinderConnection Create(Port source, Port target) {

            if ( !CanBeBoundVerbosed(source, target, null, out string verbose) ) {
                ParadoxNotion.Services.Logger.LogWarning(verbose, LogTag.EDITOR, source.parent);
                return null;
            }

            ParadoxNotion.Design.UndoUtility.RecordObject(source.parent.graph, "Connect Ports");

            BinderConnection binder = null;
            if ( source is FlowOutput && target is FlowInput ) {
                binder = new BinderConnection();
            }

            if ( source is ValueOutput && target is ValueInput ) {
                binder = (BinderConnection)System.Activator.CreateInstance(typeof(BinderConnection<>).RTMakeGenericType(new System.Type[] { target.type }));
            }

            if ( binder != null ) {

                binder.sourcePortID = source.ID;
                binder.targetPortID = target.ID;

                binder.SetSourceNode(source.parent);
                binder.SetTargetNode(target.parent);

                binder.sourcePort.connections++;
                binder.targetPort.connections++;

                binder.sourcePort.parent.OnPortConnected(binder.sourcePort, binder.targetPort);
                binder.targetPort.parent.OnPortConnected(binder.targetPort, binder.sourcePort);

                //for live editing
                if ( Application.isPlaying ) {
                    binder.Bind();
                }
            }

            ParadoxNotion.Design.UndoUtility.SetDirty(source.parent.graph);
            return binder;
        }

        ///Set binder source port
        public void SetSourcePort(Port newSourcePort) {
            if ( newSourcePort == sourcePort ) {
                return;
            }

            if ( newSourcePort == null || !newSourcePort.IsOutputPort() ) {
                return;
            }

            if ( sourcePort != null ) {
                sourcePort.connections--;
                sourcePort.parent.OnPortDisconnected(sourcePort, targetPort);
                if ( Application.isPlaying ) {
                    UnBind();
                }
            }

            _sourcePort = null; //flush
            sourcePortID = newSourcePort.ID;
            base.SetSourceNode(newSourcePort.parent);
            GatherAndValidateSourcePort();
            newSourcePort.parent.OnPortConnected(newSourcePort, targetPort);
            if ( Application.isPlaying ) {
                Bind();
            }
        }

        ///Set binder target port
        public void SetTargetPort(Port newTargetPort) {
            if ( newTargetPort == targetPort ) {
                return;
            }

            if ( newTargetPort == null || !newTargetPort.IsInputPort() ) {
                return;
            }

            if ( targetPort != null ) {
                targetPort.connections--;
                targetPort.parent.OnPortDisconnected(targetPort, sourcePort);
                if ( Application.isPlaying ) {
                    UnBind();
                }
            }

            _targetPort = null; //flush
            targetPortID = newTargetPort.ID;
            base.SetTargetNode(newTargetPort.parent);
            GatherAndValidateTargetPort();
            newTargetPort.parent.OnPortConnected(newTargetPort, sourcePort);
            if ( Application.isPlaying ) {
                Bind();
            }
        }

        ///----------------------------------------------------------------------------------------------

        ///Called after the node has GatherPorts to gather the references and validate the binding connection
        public void GatherAndValidateSourcePort() {
            _sourcePort = null; //refetch
            if ( sourcePort != null ) {
                //all good
                if ( TypeConverter.HasConvertion(sourcePort.type, bindingType) ) {
                    sourcePortID = sourcePort.ID;
                    sourcePort.connections++;
                    return;
                }
                //the cast is invalid
                Logger.LogError(string.Format("Output Port with ID '{0}' cast is invalid.", sourcePortID), LogTag.VALIDATION, sourceNode);
                sourcePort.FlagInvalidCast();
                sourcePortID = sourcePort.ID;
                sourcePort.connections++;
                return;
            }

            //the id is missing...
            Logger.LogError(string.Format("Output Port with ID '{0}' is missing on node {1}", sourcePortID, sourceNode.name), LogTag.VALIDATION, sourceNode);
            var source = sourceNode as FlowNode;
            Port missingPort = null;
            if ( bindingType == typeof(Flow) ) {
                missingPort = source.AddFlowOutput(sourcePortID, sourcePortID);
            } else {
                missingPort = source.AddValueOutput(sourcePortID, sourcePortID, typeof(object), () => { throw new System.Exception("Port is missing"); });
            }
            missingPort.FlagMissing();
            missingPort.connections++;
        }

        ///Called after the node has GatherPorts to gather the references and validate the binding connection
        public void GatherAndValidateTargetPort() {
            _targetPort = null; //refetch
            if ( targetPort != null ) {
                //all good
                if ( targetPort.type == bindingType ) {
                    targetPortID = targetPort.ID;
                    targetPort.connections++;
                    return;
                }

                //replace binder connection type if possible
                if ( targetPort is ValueInput && sourcePort is ValueOutput ) {
                    if ( TypeConverter.HasConvertion(sourcePort.type, targetPort.type) ) {
                        graph.RemoveConnection(this);
                        Create(sourcePort, targetPort);
                        targetPortID = targetPort.ID;
                        targetPort.connections++;
                        return;
                    }
                    //the cast is invalid
                    Logger.LogError(string.Format("Input Port with ID '{0}' cast is invalid.", targetPortID), LogTag.VALIDATION, targetNode);
                    targetPort.FlagInvalidCast();
                    targetPortID = targetPort.ID;
                    targetPort.connections++;
                    return;
                }
            }

            //the id is missing...
            Logger.LogError(string.Format("Input Port with ID '{0}' is missing on node {1}", targetPortID, targetNode.name), LogTag.VALIDATION, targetNode);
            var target = targetNode as FlowNode;
            Port missingPort = null;
            if ( bindingType == typeof(Flow) ) {
                missingPort = target.AddFlowInput(targetPortID, targetPortID, (f) => { throw new System.Exception("Port is missing"); });
            } else {
                missingPort = target.AddValueInput(targetPortID, targetPortID, typeof(object));
            }
            missingPort.FlagMissing();
            missingPort.connections++;
        }

        ///----------------------------------------------------------------------------------------------

        ///Return whether or not source can connect to target.
        public static bool CanBeBound(Port source, Port target, BinderConnection refConnection) { return CanBeBoundVerbosed(source, target, refConnection, out string v); }
        ///Return whether or not source can connect to target. The return is a string with the reason why NOT, null if possible.
        ///Providing an existing ref connection, will bypass source/target validation respectively if that connection is already connected to that source/target port.
        public static bool CanBeBoundVerbosed(Port source, Port target, BinderConnection refConnection, out string verbose) {
            verbose = CanBeBoundVerbosed_Internal(source, target, refConnection);
            return verbose == null;
        }

        //...
        static string CanBeBoundVerbosed_Internal(Port source, Port target, BinderConnection refConnection) {
            if ( source == null || target == null ) {
                return "A port is null.";
            }

            if ( source == target ) {
                // return "Can't connect port to itself.";
                return string.Empty;
            }

            if ( source.parent == target.parent ) {
                return "Can't connect ports on the same node.";
            }

            if ( source.parent == target.parent ) {
                return "Can't connect ports on the same parent node.";
            }

            if ( source.IsInputPort() && target.IsInputPort() ) {
                return "Can't connect input to input.";
            }

            if ( source.IsOutputPort() && target.IsOutputPort() ) {
                return "Can't connect output to output.";
            }

            if ( source.IsFlowPort() != target.IsFlowPort() ) {
                return "Flow ports can only be connected to other Flow ports.";
            }

            if ( refConnection == null || refConnection.sourcePort != source ) {
                if ( !source.CanAcceptConnections() ) {
                    return "Source port can accept no more out connections.";
                }
            }

            if ( refConnection == null || refConnection.targetPort != target ) {
                if ( !target.CanAcceptConnections() ) {
                    return "Target port can accept no more in connections.";
                }
            }

            if ( !TypeConverter.HasConvertion(source.type, target.type) ) {
                return string.Format("Can't connect ports. Type '{0}' is not assignable from Type '{1}' and there exists no automatic conversion for those types.", target.type.FriendlyName(), source.type.FriendlyName());
            }

            return null;
        }

        ///Callback from base class. The connection reference is already removed from target and source Nodes
        public override void OnDestroy() {
            if ( sourcePort != null ) {
                sourcePort.connections--;
                sourcePort.parent.OnPortDisconnected(sourcePort, targetPort);
            }
            if ( targetPort != null ) {
                targetPort.connections--;
                targetPort.parent.OnPortDisconnected(targetPort, sourcePort);
            }

            //for live editing
            if ( Application.isPlaying ) {
                UnBind();
            }
        }

        ///Called in runtime intialize to actualy bind the delegates
        virtual public void Bind() {

            if ( !isActive ) {
                return;
            }

            if ( sourcePort is FlowOutput && targetPort is FlowInput ) {
                ( sourcePort as FlowOutput ).BindTo((FlowInput)targetPort);

#if UNITY_EDITOR && DO_EDITOR_BINDING
                ( sourcePort as FlowOutput ).Append(BlinkStatus);
#else
                sourcePort.connections++;
                targetPort.connections++;
#endif
            }
        }

        ///UnBinds the delegates
        virtual public void UnBind() {
            if ( sourcePort is FlowOutput ) {
                ( sourcePort as FlowOutput ).UnBind();
            }
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        private int lastBlinkFrame;

        public override TipConnectionStyle tipConnectionStyle { get { return TipConnectionStyle.None; } }
        public override ParadoxNotion.PlanarDirection direction { get { return ParadoxNotion.PlanarDirection.Horizontal; } }

        public override Color defaultColor {
            get { return bindingType == typeof(Flow) ? base.defaultColor : Color.grey; }
            // get { return bindingType == typeof(Flow) ? Color.white : ParadoxNotion.Design.TypePrefs.GetTypeColor(bindingType); }
        }

        public override float defaultSize {
            get { return bindingType == typeof(Flow) ? base.defaultSize + 1 : base.defaultSize; }
        }

        //...
        sealed protected override string GetConnectionInfo() {

            var case1 = sourcePort == null || sourcePort.bindStatus != Port.BindStatus.Valid;
            var case2 = targetPort == null || targetPort.bindStatus != Port.BindStatus.Valid;
            if ( case1 || case2 ) { return null; }

            if ( targetPort.willDraw ) {

                if ( Application.isPlaying ) {
                    return GetTransferDataLabel();
                }

                if ( !targetPort.type.IsAssignableFrom(sourcePort.type) ) {
                    return "<size=14>➲</size>";
                }
            }

            return null;
        }

        //Data label to show on binder info
        virtual protected string GetTransferDataLabel() { return null; }

        //...
        protected override void OnConnectionInspectorGUI() {
            GUI.color = GUI.color.WithAlpha(0.5f);
            GUILayout.Label(string.Format("Binding Type Of {0}", bindingType.FriendlyName()));
            GUI.color = Color.white;

            if ( sourcePort == null || sourcePort.bindStatus != Port.BindStatus.Valid ) {
                UnityEditor.EditorGUILayout.HelpBox(string.Format("Source Port with ID '{0}' is {1} on source node.\nYou should relink the connection or simply remove it.", sourcePortID, sourcePort?.bindStatus), UnityEditor.MessageType.Error);
            }
            if ( targetPort == null || targetPort.bindStatus != Port.BindStatus.Valid ) {
                UnityEditor.EditorGUILayout.HelpBox(string.Format("Target Port with ID '{0}' is {1} on target node.\nYou should relink the connection or simply remove it.", targetPortID, targetPort?.bindStatus), UnityEditor.MessageType.Error);
            }
        }

        //...
        protected override string GetError() {
            var case1 = sourcePort == null || sourcePort.bindStatus != Port.BindStatus.Valid;
            var case2 = targetPort == null || targetPort.bindStatus != Port.BindStatus.Valid;
            if ( case1 || case2 ) { return "Port is not valid;"; }
            return null;
        }

        ///Blinks connection status
        protected void BlinkStatus(Flow f = default(Flow)) {
            if ( Application.isPlaying ) {
                lastBlinkFrame = graph.lastUpdateFrame;
                status = Status.Running;
            }
        }

        //reset it back to resting
        protected override void OnBeforeUpdateBlinkStatus() {
            if ( Application.isPlaying ) {
                //give +1 frame forward to handle potential different execution order
                if ( graph.lastUpdateFrame > lastBlinkFrame + 1 || !graph.isRunning ) {
                    status = Status.Resting;
                }
            }
        }

#endif
        ///----------------------------------------------------------------------------------------------

    }
}