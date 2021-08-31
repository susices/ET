using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ParadoxNotion;
using NodeCanvas.Framework;
using FlowCanvas.Macros;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using Logger = ParadoxNotion.Services.Logger;

namespace FlowCanvas
{

    ///Base class for flow graphs.
    [GraphInfo(
        packageName = "FlowCanvas",
        docsURL = "http://flowcanvas.paradoxnotion.com/documentation/",
        resourcesURL = "http://flowcanvas.paradoxnotion.com/downloads/",
        forumsURL = "http://flowcanvas.paradoxnotion.com/forums-page/"
        )]
    [System.Serializable]
    abstract public class FlowGraph : Graph
    {
        private List<IUpdatable> updatableNodes;
        private List<MacroNodeWrapper> macroWrappers;
        private Dictionary<string, IInvokable> functions;
        private Dictionary<System.Type, Component> cachedAgentComponents;

        public override System.Type baseNodeType => typeof(FlowNode);
        public override bool allowBlackboardOverrides => true;
        public override bool requiresAgent => false;
        public override bool requiresPrimeNode => false;
        public override bool isTree => false;
        public override bool canAcceptVariableDrops => true;

        ///Calls and returns a value of a custom function in the flowgraph
        public T CallFunction<T>(string name, params object[] args) {
            return (T)CallFunction(name, args);
        }

        ///Calls and returns a value of a custom function in the flowgraph
        public object CallFunction(string name, params object[] args) {
            Debug.Assert(isRunning, "Trying to Execute Function but graph is not running");
            IInvokable func = null;
            if ( functions.TryGetValue(name, out func) ) {
                return func.Invoke(args);
            }
            return null;
        }

        ///Calls a custom function in the flowgraph async. When the function is done, it will callback with return value
        public void CallFunctionAsync(string name, System.Action<object> callback, params object[] args) {
            Debug.Assert(isRunning, "Trying to Execute Function but graph is not running");
            IInvokable func = null;
            if ( functions.TryGetValue(name, out func) ) {
                func.InvokeAsync(callback, args);
            }
        }

        ///Returns cached component type from graph agent
        public UnityEngine.Object GetAgentComponent(System.Type type) {
            if ( agent == null ) { return null; }
            if ( type == typeof(GameObject) ) { return agent.gameObject; }
            if ( type == typeof(Transform) ) { return agent.transform; }
            if ( type == typeof(Component) ) { return agent; }

            if ( cachedAgentComponents == null ) {
                cachedAgentComponents = new Dictionary<System.Type, Component>();
            }

            Component component = null;
            if ( cachedAgentComponents.TryGetValue(type, out component) ) {
                return component;
            }

            if ( typeof(Component).RTIsAssignableFrom(type) || type.RTIsInterface() ) {
                component = agent.GetComponent(type);
            }

            return cachedAgentComponents[type] = component;
        }

        //...
        protected override void OnGraphInitialize() {
            updatableNodes = new List<IUpdatable>();
            macroWrappers = new List<MacroNodeWrapper>();
            functions = new Dictionary<string, IInvokable>(System.StringComparer.Ordinal);

            for ( var i = 0; i < allNodes.Count; i++ ) {
                var node = allNodes[i];
                if ( node is MacroNodeWrapper ) {
                    var macroWrapper = (MacroNodeWrapper)node;
                    if ( macroWrapper.macro != null ) {
                        macroWrappers.Add(macroWrapper);
                        ThreadSafeInitCall(macroWrapper.MakeInstance);
                    }
                }

                if ( node is IUpdatable ) {
                    updatableNodes.Add((IUpdatable)node);
                }

                if ( node is IInvokable ) {
                    var func = (IInvokable)node;
                    functions[func.GetInvocationID()] = func;
                }
            }

            //2nd pass after macros have been instanced
            ThreadSafeInitCall(InitSecondPass);
        }

        void InitSecondPass() {
            for ( var i = 0; i < allNodes.Count; i++ ) {
                if ( allNodes[i] is FlowNode ) {
                    var flowNode = (FlowNode)allNodes[i];
                    flowNode.BindPorts();
                    flowNode.AssignSelfInstancePort();
                }
            }
        }

        //...
        protected override void OnGraphStarted() {
            for ( var i = 0; i < macroWrappers.Count; i++ ) {
                var macroWrapper = macroWrappers[i];
                if ( macroWrapper.macro != null ) {
                    macroWrapper.macro.StartGraph(agent, blackboard.parent, Graph.UpdateMode.Manual, null);
                }
            }
        }

        //Update IUpdatable nodes. Basicaly for events like Input, Update etc
        //This is the only thing that updates per-frame
        protected override void OnGraphUpdate() {
            if ( updatableNodes != null && updatableNodes.Count > 0 ) {
                for ( var i = 0; i < updatableNodes.Count; i++ ) {
                    updatableNodes[i].Update();
                }
            }
        }

        //...
        protected override void OnGraphStoped() {
            for ( var i = 0; i < allNodes.Count; i++ ) {
                var node = allNodes[i];
                if ( node is MacroNodeWrapper ) {
                    var macroWrapper = (MacroNodeWrapper)node;
                    if ( macroWrapper.macro != null ) {
                        macroWrapper.macro.Stop();
                    }
                }
            }
        }



        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        //Append menu items in canvas right click context menu (provided menu is completely overriden here)
        protected override UnityEditor.GenericMenu OnCanvasContextMenu(UnityEditor.GenericMenu menu, Vector2 mousePos) {
            return FlowGraphExtensions.GetFullNodesMenu(this, mousePos, null, null);
        }

        //Append ConvertToMacro feature
        protected override UnityEditor.GenericMenu OnNodesContextMenu(UnityEditor.GenericMenu menu, Node[] nodes) {
            menu.AddItem(new GUIContent("Convert To Macro"), false, () => { FlowGraphExtensions.ConvertNodesToMacro(nodes.ToList()); });
            return menu;
        }

        //Append buttons to toolbar
        protected override void OnGraphEditorToolbar() {
            UnityEditor.EditorGUIUtility.SetIconSize(new Vector2(14, 14));
            if ( GUILayout.Button(EditorUtils.GetTempContent(NodeCanvas.Editor.StyleSheet.verboseLevel1, "Minimize All/Selected"), UnityEditor.EditorStyles.toolbarButton) ) {
                foreach ( var node in NodeCanvas.Editor.GraphEditorUtility.GetSelectedOrAll(this).OfType<FlowNode>() ) {
                    node.verboseLevel = Node.VerboseLevel.Compact;
                }
            }
            if ( GUILayout.Button(EditorUtils.GetTempContent(NodeCanvas.Editor.StyleSheet.verboseLevel2, "Connected Ports Only on All/Selected"), UnityEditor.EditorStyles.toolbarButton) ) {
                foreach ( var node in NodeCanvas.Editor.GraphEditorUtility.GetSelectedOrAll(this).OfType<FlowNode>() ) {
                    node.verboseLevel = Node.VerboseLevel.Partial;
                }
            }
            if ( GUILayout.Button(EditorUtils.GetTempContent(NodeCanvas.Editor.StyleSheet.verboseLevel3, "Maximize All/Selected\n(You can also hold S Key over a node to temporarily maximize it)"), UnityEditor.EditorStyles.toolbarButton) ) {
                foreach ( var node in NodeCanvas.Editor.GraphEditorUtility.GetSelectedOrAll(this).OfType<FlowNode>() ) {
                    node.verboseLevel = Node.VerboseLevel.Full;
                }
            }
            UnityEditor.EditorGUIUtility.SetIconSize(Vector2.zero);
        }

        //Create and set a UnityObject variable node on drop
        protected override void OnDropAccepted(Object o, Vector2 mousePos) {

            if ( o == null ) {
                return;
            }

            if ( UnityEditor.EditorUtility.IsPersistent(this) && !UnityEditor.EditorUtility.IsPersistent(o) ) {
                Logger.Log("This Graph is an asset. The dragged object is a scene reference. The reference will not persist!", LogTag.EDITOR);
            }

            var targetType = o.GetType();
            var menu = new UnityEditor.GenericMenu();
            menu = AppendDragAndDropObjectMenu(menu, o, "", mousePos);
            menu.AddSeparator("/");
            menu = this.AppendTypeReflectionNodesMenu(menu, targetType, "", mousePos, null, o);
            if ( o is GameObject ) {
                foreach ( var component in ( o as GameObject ).GetComponents<Component>().Where(c => c.hideFlags == 0) ) {
                    var cType = component.GetType();
                    menu = AppendDragAndDropObjectMenu(menu, component, cType.Name + "/", mousePos);
                    menu = this.AppendTypeReflectionNodesMenu(menu, cType, "", mousePos, null, component);
                }
            }

            menu.ShowAsBrowser("Add Node For Drag & Drop Instance");
            Event.current.Use();
        }

        //Used above for convenience
        UnityEditor.GenericMenu AppendDragAndDropObjectMenu(UnityEditor.GenericMenu menu, UnityEngine.Object o, string category, Vector2 mousePos) {
            foreach ( var _wrapperType in NodeCanvas.Editor.GraphEditorUtility.GetDropedReferenceNodeTypes<IDropedReferenceNode>(o) ) {
                var wrapperType = _wrapperType;
                if ( baseNodeType.IsAssignableFrom(wrapperType) ) {
                    menu.AddItem(new GUIContent(string.Format(category + "Add Node ({0})", wrapperType.FriendlyName())), false, (x) => { this.AddDropedReferenceNode(wrapperType, mousePos, null, (UnityEngine.Object)x); }, o);
                }
            }

            if ( o is IExternalImplementedNode ) {
                menu.AddItem(new GUIContent(category + "Add Implemented Node"), false, (x) => { this.AddExternalImplementedNodeWrapper(mousePos, null, (IExternalImplementedNode)x); }, o);
            }

            var targetType = o.GetType();
            menu.AddItem(new GUIContent(string.Format(category + "Make Variable ({0})", targetType.FriendlyName())), false, (x) => { this.AddVariableGet(targetType, null, null, mousePos, null, x); }, o);
            return menu;
        }

        ///Show Get/Set variable menu
        protected override void OnVariableDropInGraph(IBlackboard bb, Variable variable, Vector2 mousePos) {
            if ( variable != null ) {
                var menu = new UnityEditor.GenericMenu();
                menu.AddItem(new GUIContent("Get " + variable.name), false, () => { this.AddVariableGet(variable.varType, bb, variable, mousePos, null, null); });
                menu.AddItem(new GUIContent("Set " + variable.name), false, () => { this.AddVariableSet(variable.varType, bb, variable, mousePos, null, null); });
                menu.ShowAsContext();
                Event.current.Use();
            }
        }

#endif
        ///----------------------------------------------------------------------------------------------

    }
}