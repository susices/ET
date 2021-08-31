#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;
using ParadoxNotion;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using NodeCanvas.Editor;
using FlowCanvas.Nodes;
using FlowCanvas.Macros;
using System.Text;

namespace FlowCanvas
{

    public static class FlowGraphExtensions
    {

        //...
        public static T AddFlowNode<T>(this FlowGraph graph, Vector2 pos, Port context, object dropInstance) where T : FlowNode {
            return (T)AddFlowNode(graph, typeof(T), pos, context, dropInstance);
        }

        //...
        public static FlowNode AddFlowNode(this FlowGraph graph, System.Type type, Vector2 pos, Port context, object dropInstance) {
            if ( type.IsGenericTypeDefinition ) { type = type.MakeGenericType(type.GetFirstGenericParameterConstraintType()); }
            var node = (FlowNode)graph.AddNode(type, pos);
            Finalize(node, context, dropInstance);
            return node;
        }

        ///----------------------------------------------------------------------------------------------

        //...
        public static IDropedReferenceNode AddDropedReferenceNode(this FlowGraph graph, System.Type type, Vector2 pos, Port context, UnityEngine.Object dropInstance) {
            var node = (IDropedReferenceNode)graph.AddNode(type, pos);
            node.SetTarget(dropInstance);
            Finalize(node as FlowNode, context, dropInstance);
            return node;
        }

        //...
        public static ExternalImplementedNodeWrapper AddExternalImplementedNodeWrapper(this FlowGraph graph, Vector2 pos, Port context, IExternalImplementedNode dropInstance) {
            var node = graph.AddNode<ExternalImplementedNodeWrapper>(pos);
            node.SetTarget(dropInstance);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static ParameterVariableNode AddVariableGet(this FlowGraph graph, System.Type varType, IBlackboard bb, Variable variable, Vector2 pos, Port context, object dropInstance) {
            var genericType = typeof(GetVariable<>).MakeGenericType(varType);
            var node = (ParameterVariableNode)graph.AddNode(genericType, pos);
            node.parameter.SetTargetVariable(bb, variable);
            if ( dropInstance != null ) { node.parameter.value = dropInstance; }
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static ParameterVariableNode AddVariableSet(this FlowGraph graph, System.Type varType, IBlackboard bb, Variable variable, Vector2 pos, Port context, object dropInstance) {
            var genericType = typeof(SetVariable<>).MakeGenericType(varType);
            var node = (ParameterVariableNode)graph.AddNode(genericType, pos);
            node.parameter.SetTargetVariable(bb, variable);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static FlowNode AddSimplexNode(this FlowGraph graph, System.Type type, Vector2 pos, Port context, object dropInstance) {
            if ( type.IsGenericTypeDefinition ) { type = type.MakeGenericType(type.GetFirstGenericParameterConstraintType()); }
            var genericType = typeof(SimplexNodeWrapper<>).MakeGenericType(type);
            var node = (FlowNode)graph.AddNode(genericType, pos);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static MacroNodeWrapper AddMacroNode(this FlowGraph graph, Macro m, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<MacroNodeWrapper>(pos);
            node.macro = m;
            Finalize(node, context, dropInstance);
            return node;
        }

        public static InvokeSignal AddSignalInvokeNode(this FlowGraph graph, SignalDefinition s, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<InvokeSignal>(pos);
            node.SetTarget(s);
            Finalize(node, context, dropInstance);
            return node;
        }

        public static SignalCallback AddSignalCallbackNode(this FlowGraph graph, SignalDefinition s, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<SignalCallback>(pos);
            node.SetTarget(s);
            Finalize(node, context, dropInstance);
            return node;
        }

        ///----------------------------------------------------------------------------------------------

        //...
        public static ReflectedConstructorNodeWrapper AddContructorNode(this FlowGraph graph, ConstructorInfo c, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<ReflectedConstructorNodeWrapper>(pos);
            node.SetMethodBase(c);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static ReflectedMethodNodeWrapper AddMethodNode(this FlowGraph graph, MethodInfo m, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<ReflectedMethodNodeWrapper>(pos);
            node.SetMethodBase(m);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static ReflectedFieldNodeWrapper AddFieldGetNode(this FlowGraph graph, FieldInfo f, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<ReflectedFieldNodeWrapper>(pos);
            node.SetField(f, ReflectedFieldNodeWrapper.AccessMode.GetField);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static ReflectedFieldNodeWrapper AddFieldSetNode(this FlowGraph graph, FieldInfo f, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<ReflectedFieldNodeWrapper>(pos);
            node.SetField(f, ReflectedFieldNodeWrapper.AccessMode.SetField);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static UnityEventAutoCallbackEvent AddUnityEventAutoCallbackNode(this FlowGraph graph, MemberInfo fieldOrProp, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<UnityEventAutoCallbackEvent>(pos);
            node.SetEvent(fieldOrProp, dropInstance);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static CSharpAutoCallbackEvent AddCSharpEventAutoCallbackNode(this FlowGraph graph, EventInfo info, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<CSharpAutoCallbackEvent>(pos);
            node.SetEvent(info, dropInstance);
            Finalize(node, context, dropInstance);
            return node;
        }

        public static GetSharpEvent AddCSharpGetNode(this FlowGraph graph, EventInfo info, Vector2 pos, Port context, object dropInstance) {
            var node = graph.AddNode<GetSharpEvent>(pos);
            node.SetEvent(info);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static FlowNode AddSimplexExtractorNode(this FlowGraph graph, System.Type type, Vector2 pos, Port context, object dropInstance) {
            var simplexWrapper = typeof(SimplexNodeWrapper<>).MakeGenericType(type);
            var node = (FlowNode)graph.AddNode(simplexWrapper, pos);
            Finalize(node, context, dropInstance);
            return node;
        }

        //...
        public static FlowNode AddReflectedExtractorNode(this FlowGraph graph, System.Type type, Vector2 pos, Port context, object dropInstance) {
            var genericType = typeof(ReflectedExtractorNodeWrapper<>).MakeGenericType(type);
            var node = (FlowNode)graph.AddNode(genericType, pos);
            Finalize(node, context, dropInstance);
            return node;
        }


        ///----------------------------------------------------------------------------------------------

        static void Finalize(FlowNode node, Port context, object dropInstance) {
            FinalizeConnection(context, node);
            DropInstance(node, dropInstance);
            GraphEditorUtility.activeElement = node;
        }

        //...
        static void FinalizeConnection(Port context, FlowNode targetNode) {
            if ( context == null || targetNode == null ) {
                return;
            }

            Port source = null;
            Port target = null;

            if ( context is ValueOutput || context is FlowOutput ) {
                source = context;
                target = targetNode.GetFirstInputOfType(context.type);
            } else {
                source = targetNode.GetFirstOutputOfType(context.type);
                target = context;
            }

            BinderConnection.Create(source, target);
        }

        //...
        static void DropInstance(FlowNode targetNode, object dropInstance) {
            if ( targetNode == null || dropInstance == null ) {
                return;
            }

            //dont set instance if it's 'Self'
            if ( dropInstance is UnityEngine.Object ) {
                var ownerGO = targetNode.graph.agent != null ? targetNode.graph.agent.gameObject : null;
                if ( ownerGO != null ) {
                    var dropGO = dropInstance as GameObject;
                    if ( dropGO == ownerGO ) {
                        return;
                    }
                    var dropComp = dropInstance as Component;
                    if ( dropComp != null && dropComp.gameObject == ownerGO ) {
                        return;
                    }
                }
            }

            var instancePort = targetNode.GetFirstInputOfType(dropInstance.GetType()) as ValueInput;
            if ( instancePort != null ) {
                instancePort.serializedValue = dropInstance;
            }
        }


        ///----------------------------------------------------------------------------------------------


        ///Returns all nodes' menu
        public static UnityEditor.GenericMenu GetFullNodesMenu(this FlowGraph flowGraph, Vector2 mousePos, Port context, Object dropInstance) {
            var menu = new UnityEditor.GenericMenu();
            if ( context is ValueInput || context is ValueOutput ) {
                menu = flowGraph.AppendTypeReflectionNodesMenu(menu, context.type, "", mousePos, context, dropInstance);
            }
            menu = flowGraph.AppendFlowNodesMenu(menu, "", mousePos, context, dropInstance);
            menu = flowGraph.AppendSimplexNodesMenu(menu, "Functions/Implemented", mousePos, context, dropInstance);
            menu = flowGraph.AppendAllReflectionNodesMenu(menu, "Functions/Reflected", mousePos, context, dropInstance);
            menu = flowGraph.AppendVariableNodesMenu(menu, "Variables", mousePos, context, dropInstance);
            menu = flowGraph.AppendSignals(menu, "Events/Signals", mousePos, context, dropInstance);
            menu = flowGraph.AppendMacroNodesMenu(menu, "MACROS", mousePos, context, dropInstance);
            menu = flowGraph.AppendMenuCallbackReceivers(menu, "", mousePos, context, dropInstance);
            return menu;
        }


        ///----------------------------------------------------------------------------------------------

        //FlowNode
        public static UnityEditor.GenericMenu AppendFlowNodesMenu(this FlowGraph graph, UnityEditor.GenericMenu menu, string baseCategory, Vector2 pos, Port contextPort, object dropInstance) {
            var infos = EditorUtils.GetScriptInfosOfType(typeof(FlowNode));
            var generalized = new List<System.Type>();
            foreach ( var _info in infos ) {
                var info = _info;
                if ( contextPort != null ) {

                    if ( generalized.Contains(info.originalType) ) {
                        continue;
                    }

                    if ( contextPort.IsValuePort() && info.originalType.IsGenericTypeDefinition ) {
                        var genericInfo = info.MakeGenericInfo(contextPort.type);
                        if ( genericInfo.isValid ) {
                            info = genericInfo;
                            generalized.Add(info.originalType);
                        }
                    }

                    IEnumerable<System.Type> attributeDefinedTypes = new System.Type[0];
                    if ( contextPort.IsOutputPort() ) {
                        var att = info.type.RTGetAttribute<FlowNode.ContextDefinedInputsAttribute>(true);
                        if ( att != null ) { attributeDefinedTypes = att.types.Select(t => t == typeof(Wild) && info.type.IsGenericType ? info.type.RTGetGenericArguments().First() : t); }
                    }

                    if ( contextPort.IsInputPort() ) {
                        var att = info.type.RTGetAttribute<FlowNode.ContextDefinedOutputsAttribute>(true);
                        if ( att != null ) { attributeDefinedTypes = att.types.Select(t => t == typeof(Wild) && info.type.IsGenericType ? info.type.RTGetGenericArguments().First() : t); }
                    }

                    if ( contextPort is ValueOutput ) {
                        var portTypes = info.type.RTGetFields()
                        .Where(f => f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(ValueInput<>))
                        .Select(f => f.FieldType.RTGetGenericArguments().First())
                        .Union(attributeDefinedTypes);
                        if ( !portTypes.Any(t => t.IsAssignableFrom(contextPort.type)) ) { continue; }
                    }

                    if ( contextPort is ValueInput ) {
                        var portTypes = info.type.RTGetFields()
                        .Where(f => f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(ValueOutput<>))
                        .Select(f => f.FieldType.RTGetGenericArguments().First())
                        .Union(attributeDefinedTypes);
                        if ( !portTypes.Any(t => contextPort.type.IsAssignableFrom(t)) ) { continue; }
                    }

                    if ( contextPort is FlowOutput ) {
                        var portTypes = info.type.RTGetFields().Select(f => f.FieldType).Union(attributeDefinedTypes);
                        if ( !portTypes.Any(t => t == typeof(FlowInput) || t == typeof(Flow)) ) { continue; }
                    }

                    if ( contextPort is FlowInput ) {
                        var portTypes = info.type.RTGetFields().Select(f => f.FieldType).Union(attributeDefinedTypes);
                        if ( !portTypes.Any(t => t == typeof(FlowOutput) || t == typeof(Flow)) ) { continue; }
                    }
                }

                var category = string.Join("/", new string[] { baseCategory, info.category, info.name }).TrimStart('/');
                menu.AddItem(new GUIContent(category), false, (o) => { graph.AddFlowNode((System.Type)o, pos, contextPort, dropInstance); }, info.type);
            }
            return menu;
        }

        ///----------------------------------------------------------------------------------------------

        ///Simplex Nodes
        public static UnityEditor.GenericMenu AppendSimplexNodesMenu(this FlowGraph graph, UnityEditor.GenericMenu menu, string baseCategory, Vector2 pos, Port contextPort, object dropInstance) {
            var infos = EditorUtils.GetScriptInfosOfType(typeof(SimplexNode));
            var generalized = new List<System.Type>();
            foreach ( var _info in infos ) {
                var info = _info;
                if ( contextPort != null ) {

                    if ( generalized.Contains(info.originalType) ) {
                        continue;
                    }

                    if ( contextPort.IsValuePort() && info.originalType.IsGenericTypeDefinition ) {
                        var genericInfo = info.MakeGenericInfo(contextPort.type);
                        if ( genericInfo.isValid ) {
                            info = genericInfo;
                            generalized.Add(info.originalType);
                        }
                    }

                    var invokeMethod = info.type.RTGetMethod("Invoke");
                    if ( invokeMethod != null ) {
                        if ( contextPort is ValueOutput ) {
                            var parameterTypes = invokeMethod.GetParameters().Select(p => p.ParameterType).ToArray();
                            if ( parameterTypes.Length == 0 || !parameterTypes.Any(t => t.IsAssignableFrom(contextPort.type)) ) {
                                continue;
                            }
                        }
                        if ( contextPort is ValueInput ) {
                            var outProperties = info.type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                            if ( !contextPort.type.IsAssignableFrom(invokeMethod.ReturnType) && !outProperties.Any(p => p.CanRead && p.CanWrite && contextPort.type.IsAssignableFrom(p.PropertyType)) ) {
                                continue;
                            }
                        }
                        if ( contextPort is FlowOutput || contextPort is FlowInput ) {
                            if ( invokeMethod.ReturnType != typeof(void) && invokeMethod.ReturnType != typeof(System.Collections.IEnumerator) ) {
                                continue;
                            }
                            if ( info.type.IsSubclassOf(typeof(ExtractorNode)) ) {
                                continue;
                            }
                        }
                    }
                }

                var category = string.Join("/", new string[] { baseCategory, info.category, info.name }).TrimStart('/');
                menu.AddItem(new GUIContent(category), false, (o) => { graph.AddSimplexNode((System.Type)o, pos, contextPort, dropInstance); }, info.type);
            }
            return menu;
        }

        ///----------------------------------------------------------------------------------------------

        ///All reflection type nodes
        public static UnityEditor.GenericMenu AppendAllReflectionNodesMenu(this FlowGraph graph, UnityEditor.GenericMenu menu, string baseCategory, Vector2 pos, Port contextPort, object dropInstance) {
            foreach ( var type in TypePrefs.GetPreferedTypesList() ) {
                menu = graph.AppendTypeReflectionNodesMenu(menu, type, baseCategory, pos, contextPort, dropInstance);
            }
            return menu;
        }

        ///Refletion nodes on a type
        public static UnityEditor.GenericMenu AppendTypeReflectionNodesMenu(this FlowGraph graph, UnityEditor.GenericMenu menu, System.Type type, string baseCategory, Vector2 pos, Port contextPort, object dropInstance) {

            var builder = new StringBuilder();

            if ( !string.IsNullOrEmpty(baseCategory) ) {
                baseCategory += "/";
            }

            var typeCategory = baseCategory + type.FriendlyName();

            if ( !type.IsPrimitive ) {
                if ( contextPort == null || ( contextPort is ValueOutput && type.IsAssignableFrom(contextPort.type) ) ) {
                    var extractName = string.Format("/Extract {0}", type.FriendlyName());
                    menu.AddItem(new GUIContent(typeCategory + extractName), false, (o) => { graph.AddReflectedExtractorNode(type, pos, contextPort, dropInstance); }, type);
                }
            }

            if ( contextPort is ValueInput ) {
                if ( contextPort.type == type ) {
                    var portValue = ( contextPort as ValueInput ).serializedValue;
                    menu.AddItem(new GUIContent("Make Constant Variable"), false, (o) => { graph.AddVariableGet((System.Type)o, null, null, pos, contextPort, portValue); }, type);
                    menu.AddItem(new GUIContent("Make Linked Variable"), false, (o) =>
                    {
                        var bbVar = graph.blackboard.AddVariable(contextPort.name, contextPort.type);
                        if ( bbVar != null ) { graph.AddVariableGet((System.Type)o, graph.blackboard, bbVar, pos, contextPort, portValue); }
                    }, type);
                    menu.AddSeparator("/");
                }
            }

            //Constructors
            if ( !type.IsAbstract && !type.IsInterface && !type.IsPrimitive && type != typeof(string) ) {
                foreach ( var _c in type.RTGetConstructors() ) {
                    var c = _c;
                    if ( !c.IsPublic || c.IsObsolete() ) {
                        continue;
                    }

                    if ( contextPort is FlowInput || contextPort is FlowOutput ) {
                        continue;
                    }

                    var parameters = c.GetParameters();
                    if ( contextPort is ValueOutput ) {
                        if ( !parameters.Any(p => p.ParameterType.IsAssignableFrom(contextPort.type)) ) {
                            continue;
                        }
                    }

                    if ( contextPort is ValueInput ) {
                        if ( !contextPort.type.IsAssignableFrom(type) ) {
                            continue;
                        }
                    }

                    builder.Clear();
                    builder.Append(typeCategory).Append("/Constructors/").Append(c.SignatureName());

                    if ( typeof(Component).IsAssignableFrom(type) ) {
                        if ( type == typeof(Transform) ) {
                            continue;
                        }
                        menu.AddItem(new GUIContent(builder.ToString()), false, (o) => { graph.AddSimplexNode(typeof(AddComponent<>).MakeGenericType(type), pos, contextPort, dropInstance); }, c);
                        continue;
                    }

                    if ( typeof(ScriptableObject).IsAssignableFrom(type) ) {
                        menu.AddItem(new GUIContent(builder.ToString()), false, (o) => { graph.AddSimplexNode(typeof(NewScriptableObject<>).MakeGenericType(type), pos, contextPort, dropInstance); }, c);
                        continue;
                    }

                    //exclude types like Mathf, Random, Time etc (they are not static)
                    if ( !TypePrefs.functionalTypesBlacklist.Contains(type) ) {
                        menu.AddItem(new GUIContent(builder.ToString()), false, (o) => { graph.AddContructorNode((ConstructorInfo)o, pos, contextPort, dropInstance); }, c);
                    }
                }
            }

            //Methods
            foreach ( var _m in type.RTGetMethods().Concat(type.GetExtensionMethods()).OrderBy(x => x.GetMethodSpecialType() + ( x.IsStatic ? 0 : 1 ) + ( x.DeclaringType == type ? 0 : 10 )) ) {
                var m = _m;
                if ( !m.IsPublic || m.IsObsolete() ) {
                    continue;
                }

                //convertions are handled automatically at a connection level
                if ( m.Name == "op_Implicit" || m.Name == "op_Explicit" ) {
                    continue;
                }

                var isGeneric = m.IsGenericMethod && m.RTGetGenericArguments().Length == 1;
                if ( isGeneric ) {
                    if ( contextPort != null && contextPort.IsValuePort() ) {
                        if ( !m.TryMakeGeneric(contextPort.type, out m) ) { continue; }
                    }
                }

                var parameters = m.GetParameters();
                var isUnityEvent = typeof(UnityEventBase).IsAssignableFrom(m.ReturnType);
                var isContextUnityEvent = contextPort != null && typeof(UnityEventBase).IsAssignableFrom(contextPort.type);
                if ( contextPort is ValueOutput ) {
                    if ( isUnityEvent && !isContextUnityEvent ) { continue; }
                    if ( type != contextPort.type || m.IsStatic ) {
                        if ( !parameters.Any(p => p.ParameterType.IsAssignableFrom(contextPort.type)) ) {
                            continue;
                        }
                    }
                }

                if ( contextPort is ValueInput ) {
                    if ( isUnityEvent && !isContextUnityEvent ) {
                        if ( !QualifyUnityEvent(m.ReturnType, contextPort) ) {
                            continue;
                        }
                    } else if ( !contextPort.type.IsAssignableFrom(m.ReturnType) && !parameters.Any(p => p.IsOut && contextPort.type.IsAssignableFrom(p.ParameterType)) ) {
                        continue;
                    }
                }

                if ( contextPort is FlowInput || contextPort is FlowOutput ) {
                    if ( m.ReturnType != typeof(void) && !isUnityEvent ) {
                        continue;
                    }
                }

                builder.Clear();
                builder.Append(typeCategory);

                var specialType = m.GetMethodSpecialType();
                if ( specialType == ReflectionTools.MethodType.Normal ) {
                    builder.Append("/Methods/");
                }
                if ( specialType == ReflectionTools.MethodType.PropertyAccessor ) {
                    ///special case unity event
                    if ( isUnityEvent ) {
                        builder.Append("/Events/").Append(m.Name.Replace("get_", string.Empty));
                        var prop = m.GetAccessorProperty();
                        if ( dropInstance == null ) {
                            //post to general events category as well for convenience
                            menu.AddItem(new GUIContent("Events/Reflected/" + prop.DeclaringType.Name + "." + prop.Name), false, (o) => { graph.AddUnityEventAutoCallbackNode((PropertyInfo)o, pos, contextPort, dropInstance); }, prop);
                        }
                        menu.AddItem(new GUIContent(builder.ToString() + " (Auto Subscribe)"), false, (o) => { graph.AddUnityEventAutoCallbackNode((PropertyInfo)o, pos, contextPort, dropInstance); }, prop);
                        menu.AddItem(new GUIContent(builder.ToString() + " (Get Reference)"), false, (o) => { graph.AddMethodNode((MethodInfo)o, pos, contextPort, dropInstance); }, m);
                        continue;

                    } else {

                        builder.Append("/Properties/");
                    }
                }
                if ( specialType == ReflectionTools.MethodType.Operator ) {
                    builder.Append("/Operators/");
                }
                if ( specialType == ReflectionTools.MethodType.Event ) {
                    builder.Append("/Events/");
                }

                if ( m.DeclaringType != type ) {
                    builder.Append(m.IsExtensionMethod() ? "Extensions/" : "Inherited/");
                }
                builder.Append(m.SignatureName());
                menu.AddItem(new GUIContent(builder.ToString()), false, (o) => { graph.AddMethodNode((MethodInfo)o, pos, contextPort, dropInstance); }, m);
            }

            //Fields
            foreach ( var _f in type.RTGetFields() ) {
                var f = _f;
                if ( !f.IsPublic || f.IsObsolete() ) {
                    continue;
                }

                var isReadOnly = f.IsReadOnly();
                var isConstant = f.IsConstant();

                var isUnityEvent = typeof(UnityEventBase).IsAssignableFrom(f.FieldType);
                var isContextUnityEvent = contextPort != null && typeof(UnityEventBase).IsAssignableFrom(contextPort.type);
                if ( contextPort is ValueOutput ) {
                    if ( isUnityEvent && !isContextUnityEvent ) { continue; }
                    if ( type != contextPort.type || isConstant ) {
                        if ( isReadOnly || !f.FieldType.IsAssignableFrom(contextPort.type) ) {
                            continue;
                        }
                    }
                }

                if ( contextPort is ValueInput ) {
                    if ( isUnityEvent && !isContextUnityEvent ) {
                        if ( !QualifyUnityEvent(f.FieldType, contextPort) ) {
                            continue;
                        }
                    } else if ( !contextPort.type.IsAssignableFrom(f.FieldType) ) {
                        continue;
                    }
                }

                if ( contextPort is FlowOutput ) {
                    if ( isUnityEvent && !isContextUnityEvent ) { continue; }
                }

                builder.Clear();
                builder.Append(typeCategory);
                builder.Append(isUnityEvent ? "/Events/" : "/Fields/");
                if ( f.DeclaringType != type ) { builder.Append("Inherited/"); }

                //Unity Event
                if ( isUnityEvent ) {
                    builder.Append(f.Name);
                    if ( dropInstance == null ) {
                        //post to general events category as well for convenience
                        menu.AddItem(new GUIContent("Events/Reflected/" + f.DeclaringType.Name + "." + f.Name), false, (o) => { graph.AddUnityEventAutoCallbackNode((FieldInfo)o, pos, contextPort, dropInstance); }, f);
                    }
                    menu.AddItem(new GUIContent(builder.ToString() + " (Auto Subscribe)"), false, (o) => { graph.AddUnityEventAutoCallbackNode((FieldInfo)o, pos, contextPort, dropInstance); }, f);
                    menu.AddItem(new GUIContent(builder.ToString() + " (Get Reference)"), false, (o) => { graph.AddFieldGetNode((FieldInfo)o, pos, contextPort, dropInstance); }, f);
                    continue;
                }

                var nameForGet = builder.ToString() + ( isConstant ? "constant " + f.Name : "Get " + f.Name );
                menu.AddItem(new GUIContent(nameForGet), false, (o) => { graph.AddFieldGetNode((FieldInfo)o, pos, contextPort, dropInstance); }, f);

                if ( !isReadOnly ) {
                    var nameForSet = builder.ToString() + "Set " + f.Name;
                    menu.AddItem(new GUIContent(nameForSet), false, (o) => { graph.AddFieldSetNode((FieldInfo)o, pos, contextPort, dropInstance); }, f);
                }
            }

            //C# Events
            foreach ( var _info in type.RTGetEvents() ) {
                var info = _info;

                if ( contextPort != null ) {
                    if ( contextPort.IsOutputPort() ) {
                        continue;
                    }
                    if ( contextPort is ValueInput ) {
                        var m = info.EventHandlerType.RTGetMethod("Invoke");
                        if ( m == null ) { continue; }
                        var parameters = m.GetParameters();
                        if ( !parameters.Any(p => contextPort.type.IsAssignableFrom(p.ParameterType)) ) {
                            continue;
                        }
                    }
                }

                builder.Clear();
                builder.Append(typeCategory).Append("/Events/").Append(info.Name);
                if ( dropInstance == null ) {
                    //post to general events category as well for convenience
                    menu.AddItem(new GUIContent("Events/Reflected/" + info.DeclaringType.Name + "." + info.Name), false, (o) => { graph.AddCSharpEventAutoCallbackNode((EventInfo)o, pos, contextPort, dropInstance); }, info);
                }
                menu.AddItem(new GUIContent(builder.ToString() + " (Auto Subscribe)"), false, (o) => { graph.AddCSharpEventAutoCallbackNode((EventInfo)o, pos, contextPort, dropInstance); }, info);
                menu.AddItem(new GUIContent(builder.ToString() + " (Get Reference)"), false, (o) => { graph.AddCSharpGetNode((EventInfo)o, pos, contextPort, dropInstance); }, info);
            }

            return menu;
        }

        //utility
        static bool QualifyUnityEvent(System.Type eventType, Port contextPort) {
            if ( contextPort == null || contextPort is FlowInput ) { return true; }
            if ( contextPort is ValueInput ) {
                var invokeMethod = eventType.RTGetMethod("Invoke");
                if ( invokeMethod == null ) { return false; }
                var parameters = invokeMethod.GetParameters();
                if ( parameters.Any(p => contextPort.type.IsAssignableFrom(p.ParameterType)) ) {
                    return true;
                }
            }
            return false;
        }

        ///----------------------------------------------------------------------------------------------

        ///Variable based nodes
        public static UnityEditor.GenericMenu AppendVariableNodesMenu(this FlowGraph graph, UnityEditor.GenericMenu menu, string baseCategory, Vector2 pos, Port contextPort, object dropInstance) {
            if ( !string.IsNullOrEmpty(baseCategory) ) {
                baseCategory += "/";
            }

            var variablesMap = new Dictionary<IBlackboard, IEnumerable<Variable>>();
            if ( graph.blackboard != null ) {
                foreach ( var actualBB in graph.blackboard.GetAllParents(true) ) {
                    variablesMap[actualBB] = actualBB.variables.Values;
                }
            }

            foreach ( var globalBB in GlobalBlackboard.GetAll() ) {
                variablesMap[globalBB] = globalBB.GetVariables();
            }

            menu.AddSeparator(baseCategory + "Blackboard/");

            foreach ( var pair in variablesMap.Reverse() ) {
                foreach ( var _bbVar in pair.Value ) {
                    var bb = pair.Key;
                    var bbVar = _bbVar;

                    //in try catch block because unity api is being called in async operation (and we cant use Application.isPlaying check here anyways)
                    try { if ( bbVar.value is VariableSeperator ) { continue; } } catch { continue; }

                    var category = baseCategory + "Blackboard/" + bb.identifier + "/";

                    if ( contextPort == null || ( contextPort is ValueInput && contextPort.type.IsAssignableFrom(bbVar.varType) ) ) {
                        var getName = string.Format("{0}Get '{1}'", category, bbVar.name);
                        menu.AddItem(new GUIContent(getName, null, "Get Variable"), false, () => { graph.AddVariableGet(bbVar.varType, bb, bbVar, pos, contextPort, dropInstance); });
                    }
                    if ( contextPort == null || contextPort is FlowOutput || ( contextPort is ValueOutput && bbVar.varType.IsAssignableFrom(contextPort.type) ) ) {
                        var setName = string.Format("{0}Set '{1}'", category, bbVar.name);
                        menu.AddItem(new GUIContent(setName, null, "Set Variable"), false, () => { graph.AddVariableSet(bbVar.varType, bb, bbVar, pos, contextPort, dropInstance); });
                    }
                }
            }
            return menu;
        }

        ///----------------------------------------------------------------------------------------------

        ///Macro Nodes
        public static UnityEditor.GenericMenu AppendMacroNodesMenu(this FlowGraph graph, UnityEditor.GenericMenu menu, string baseCategory, Vector2 pos, Port contextPort, object dropInstance) {
            var trackedAssets = AssetTracker.trackedAssets;
            if ( trackedAssets == null ) { return menu; }
            foreach ( var pair in trackedAssets.OrderBy(m => m.Key) ) {
                var macro = pair.Value as Macro;
                if ( macro == null ) { continue; }

                if ( contextPort is ValueOutput || contextPort is FlowOutput ) {
                    if ( !macro.inputDefinitions.Select(d => d.type).Any(d => d.IsAssignableFrom(contextPort.type)) ) {
                        continue;
                    }
                }

                if ( contextPort is ValueInput || contextPort is FlowInput ) {
                    if ( !macro.outputDefinitions.Select(d => d.type).Any(d => contextPort.type.IsAssignableFrom(d)) ) {
                        continue;
                    }
                }

                var category = baseCategory + ( !string.IsNullOrEmpty(macro.category) ? "/" + macro.category : "" );
                var idx1 = pair.Key.LastIndexOf('/') + 1;
                var idx2 = pair.Key.LastIndexOf('.');
                var macroName = pair.Key.Substring(idx1, idx2 - idx1);
                var name = category + "/" + macroName;

                var content = new GUIContent(name, null, macro.comments);
                if ( macro != graph ) {
                    menu.AddItem(content, false, () => { graph.AddMacroNode(macro, pos, contextPort, dropInstance); });
                } else { menu.AddDisabledItem(content); }
            }

            if ( contextPort == null ) {
                menu.AddItem(new GUIContent("MACROS/Create New...", null, "Create a new macro"), false, () =>
                {
                    var newMacro = EditorUtils.CreateAsset<Macro>();
                    if ( newMacro != null ) {
                        newMacro.AddExamplePorts();
                        var wrapper = graph.AddNode<MacroNodeWrapper>(pos);
                        wrapper.macro = newMacro;
                        UndoUtility.SetDirty(newMacro);
                        UnityEditor.AssetDatabase.SaveAssets();
                    }
                });
            }
            return menu;
        }

        ///Signal Definitions
        public static UnityEditor.GenericMenu AppendSignals(this FlowGraph graph, UnityEditor.GenericMenu menu, string baseCategory, Vector2 pos, Port contextPort, object dropInstance) {
            var trackedAssets = AssetTracker.trackedAssets;
            if ( trackedAssets == null ) { return menu; }
            menu.AddSeparator(baseCategory + "/");
            foreach ( var pair in trackedAssets.OrderBy(m => m.Key) ) {
                var signal = pair.Value as SignalDefinition;
                if ( signal == null ) { continue; }

                var category = baseCategory;
                var idx1 = pair.Key.LastIndexOf('/') + 1;
                var idx2 = pair.Key.LastIndexOf('.');
                var signalName = pair.Key.Substring(idx1, idx2 - idx1);


                if ( contextPort == null || contextPort is FlowOutput || ( contextPort is ValueOutput && signal.parameters.Any(p => p.type.IsAssignableFrom(contextPort.type)) ) ) {
                    var nameForInvoke = category + $"/Invoke Signal ({signalName})";
                    var contentInvoke = new GUIContent(nameForInvoke, null, null);
                    menu.AddItem(contentInvoke, false, () => { graph.AddSignalInvokeNode(signal, pos, contextPort, dropInstance); });
                }

                if ( contextPort == null || contextPort is FlowInput || ( contextPort is ValueInput && signal.parameters.Any(p => contextPort.type.IsAssignableFrom(p.type)) ) ) {
                    var nameForCallback = category + $"/Signal Callback ({signalName})";
                    var contentCallback = new GUIContent(nameForCallback, null, null);
                    menu.AddItem(contentCallback, false, () => { graph.AddSignalCallbackNode(signal, pos, contextPort, dropInstance); });
                }
            }
            return menu;
        }

        ///----------------------------------------------------------------------------------------------

        ///Nodes can post menu by themselves as well.
        public static UnityEditor.GenericMenu AppendMenuCallbackReceivers(this FlowGraph graph, UnityEditor.GenericMenu menu, string baseCategory, Vector2 pos, Port contextPort, object dropInstance) {
            foreach ( var node in graph.allNodes.OfType<IEditorMenuCallbackReceiver>() ) {
                node.OnMenu(menu, pos, contextPort, dropInstance);
            }
            return menu;
        }

        ///----------------------------------------------------------------------------------------------
        ///----------------------------------------------------------------------------------------------

        ///Convert nodes to macro (with a bit of hocus pocus)
        public static void ConvertNodesToMacro(List<Node> originalNodes) {

            if ( originalNodes == null || originalNodes.Count == 0 ) {
                return;
            }

            if ( !UnityEditor.EditorUtility.DisplayDialog("Convert to Macro", "This will create a new Macro out of the nodes.\nPlease note that since Macros are assets, Scene Object references will not be saved.\nThe Macro can NOT be unpacked later on.\nContinue?", "Yes", "No!") ) {
                return;
            }

            //create asset
            var newMacro = EditorUtils.CreateAsset<Macro>();
            if ( newMacro == null ) {
                return;
            }

            //undo
            var graph = (FlowScriptBase)originalNodes[0].graph;
            UndoUtility.RecordObjectComplete(graph, "Convert To Macro");

            //clone nodes
            var cloned = Graph.CloneNodes(originalNodes, newMacro, -newMacro.translation);

            //cache used ports
            var inputMergeMapSource = new Dictionary<Port, Port>();
            var inputMergeMapTarget = new Dictionary<Port, Port>();

            var outputMergeMapTarget = new Dictionary<Port, Port>();
            var outputMergeMapSource = new Dictionary<Port, Port>();


            //relink copied nodes to inside macro entry/exit
            for ( var i = 0; i < originalNodes.Count; i++ ) {
                var originalNode = originalNodes[i];
                //create macro entry node port definitions and link those to input ports of cloned nodes inside
                foreach ( var originalInputConnection in originalNode.inConnections.OfType<BinderConnection>() ) {
                    //only do stuff if link source node is not part of the clones
                    if ( originalNodes.Contains(originalInputConnection.sourceNode) ) {
                        continue;
                    }
                    Port defSourcePort = null;
                    //merge same input ports and same target ports
                    if ( !inputMergeMapSource.TryGetValue(originalInputConnection.sourcePort, out defSourcePort) ) {
                        if ( !inputMergeMapTarget.TryGetValue(originalInputConnection.targetPort, out defSourcePort) ) {
                            //remark: we use sourcePort.type instead of target port type, so that connections remain assignable
                            var def = new DynamicParameterDefinition(originalInputConnection.targetPort.name, originalInputConnection.sourcePort.type);
                            defSourcePort = newMacro.AddInputDefinition(def);
                            inputMergeMapTarget[originalInputConnection.targetPort] = defSourcePort;
                        }
                        inputMergeMapSource[originalInputConnection.sourcePort] = defSourcePort;
                    }

                    if ( defSourcePort.CanAcceptConnections() ) { //check this for case of merged FlowPorts
                        var targetPort = ( cloned[i] as FlowNode ).GetInputPort(originalInputConnection.targetPortID);
                        BinderConnection.Create(defSourcePort, targetPort);
                    }
                }

                //create macro exit node port definitions and link those to output ports of cloned nodes inside
                foreach ( var originalOutputConnection in originalNode.outConnections.OfType<BinderConnection>() ) {
                    //only do stuff if link target node is not part of the clones
                    if ( originalNodes.Contains(originalOutputConnection.targetNode) ) {
                        continue;
                    }
                    Port defTargetPort = null;
                    //merge same input ports and same target ports
                    if ( !outputMergeMapTarget.TryGetValue(originalOutputConnection.targetPort, out defTargetPort) ) {
                        if ( !outputMergeMapSource.TryGetValue(originalOutputConnection.sourcePort, out defTargetPort) ) {
                            var def = new DynamicParameterDefinition(originalOutputConnection.sourcePort.name, originalOutputConnection.sourcePort.type);
                            defTargetPort = newMacro.AddOutputDefinition(def);
                            outputMergeMapSource[originalOutputConnection.sourcePort] = defTargetPort;
                        }
                        outputMergeMapTarget[originalOutputConnection.targetPort] = defTargetPort;
                    }

                    if ( defTargetPort.CanAcceptConnections() ) { //check this for case of merged ValuePorts
                        var sourcePort = ( cloned[i] as FlowNode ).GetOutputPort(originalOutputConnection.sourcePortID);
                        BinderConnection.Create(sourcePort, defTargetPort);
                    }
                }
            }

            //Delete originals
            var originalBounds = RectUtils.GetBoundRect(originalNodes.Select(n => n.rect).ToArray());
            foreach ( var node in originalNodes.ToArray() ) {
                graph.RemoveNode(node, false);
            }

            //Create MacroWrapper. Relink macro wrapper to outside nodes
            var wrapperPos = originalBounds.center;
            wrapperPos.x = (int)wrapperPos.x;
            wrapperPos.y = (int)wrapperPos.y;
            var wrapper = graph.AddMacroNode(newMacro, wrapperPos, null, null);
            wrapper.GatherPorts();
            foreach ( var pair in inputMergeMapSource ) {
                var source = pair.Key;
                var target = wrapper.GetInputPort(pair.Value.ID);
                BinderConnection.Create(source, target);
            }
            foreach ( var pair in outputMergeMapTarget ) {
                var source = wrapper.GetOutputPort(pair.Value.ID);
                var target = pair.Key;
                BinderConnection.Create(source, target);
            }

            //organize a bit
            var clonedBounds = RectUtils.GetBoundRect(cloned.Select(n => n.rect).ToArray());
            newMacro.entry.position = new Vector2((int)( clonedBounds.xMin - 300 ), (int)clonedBounds.yMin);
            newMacro.exit.position = new Vector2((int)( clonedBounds.xMax + 300 ), (int)clonedBounds.yMin);
            newMacro.translation = -newMacro.entry.position + new Vector2(300, 300);

            //dirty
            UndoUtility.SetDirty(graph);
            UndoUtility.SetDirty(newMacro);

            //validate and save
            newMacro.Validate();
            UnityEditor.AssetDatabase.SaveAssets();
        }

    }
}

#endif