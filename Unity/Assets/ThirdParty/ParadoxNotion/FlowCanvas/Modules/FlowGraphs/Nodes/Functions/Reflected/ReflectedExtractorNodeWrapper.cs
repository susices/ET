using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{
    //Wrap public readable properties and fields for selected type
    [DoNotList]
    [Description("Chose and expose any number of fields or properties of the type. If you only require a single field / property, it's better to get that field / property directly without an Extractor.")]
    [Icon(runtimeIconTypeCallback: nameof(GetRuntimeIconType))]
    public class ReflectedExtractorNodeWrapper<T> : FlowNode
    {

        public System.Type GetRuntimeIconType() { return typeof(T); }

        private static Dictionary<string, MemberInfo> _memberInfos;
        private static List<string> _instanceMemberNames;
        private static List<string> _staticMemberNames;

        private static void PopulateInfos() {
            if ( _memberInfos != null ) { return; }
            _memberInfos = new Dictionary<string, MemberInfo>(StringComparer.Ordinal);
            _instanceMemberNames = new List<string>();
            _staticMemberNames = new List<string>();
            var targetType = typeof(T);

            foreach ( var field in targetType.RTGetFields() ) {
                if ( field == null || !field.IsPublic || field.IsObsolete() ) { continue; }
                _memberInfos[field.Name] = field;
                ( field.IsStatic ? _staticMemberNames : _instanceMemberNames ).Add(field.Name);
            }

            foreach ( var prop in targetType.RTGetProperties() ) {
                if ( prop == null || prop.IsIndexerProperty() || prop.IsObsolete() ) { continue; }
                var getter = prop.RTGetGetMethod();
                if ( getter == null || !getter.IsPublic ) { continue; }
                _memberInfos[prop.Name] = getter;
                ( getter.IsStatic ? _staticMemberNames : _instanceMemberNames ).Add(prop.Name);
            }
        }

        [SerializeField]
        private List<string> _selectedInstanceMembers;

        [NonSerialized]
        private BaseReflectedExtractorNode extractorNode;

        public override string name {
            get { return string.Format("Extract ({0})", typeof(T).FriendlyName()); }
        }

        protected override void RegisterPorts() {

            PopulateInfos();
            if ( _selectedInstanceMembers == null ) {
                _selectedInstanceMembers = new List<string>();
            }

            var final = new MemberInfo[_selectedInstanceMembers.Count];
            for ( var i = 0; i < _selectedInstanceMembers.Count; i++ ) {
                var name = _selectedInstanceMembers[i];
                if ( string.IsNullOrEmpty(name) ) { continue; }
                MemberInfo info;
                if ( !_memberInfos.TryGetValue(name, out info) ) {
                    ParadoxNotion.Services.Logger.LogError(string.Format("Field/Property named '{0}', was not found on type '{1}'", name, typeof(T)), "Extractor", this);
                }
                final[i] = info;
            }

            extractorNode = BaseReflectedExtractorNode.GetExtractorNode(typeof(T), false, final);
            if ( extractorNode != null ) {
                extractorNode.RegisterPorts(this);
            }
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
        ///----------------------------------------------------------------------------------------------
#if UNITY_EDITOR

        //...
        protected override void OnNodeInspectorGUI() {
            base.OnNodeInspectorGUI();
            EditorUtils.Separator();
            if ( _instanceMemberNames.Count <= 0 ) {
                UnityEditor.EditorGUILayout.HelpBox("This type has neither public fields nor public properties to extract.", UnityEditor.MessageType.Info);
                return;
            }
            if ( _selectedInstanceMembers == null ) { _selectedInstanceMembers = new List<string>(); }
            if ( DoListCheckout(_instanceMemberNames, ref _selectedInstanceMembers) ) {
                _selectedInstanceMembers = _selectedInstanceMembers.OrderBy(x => _instanceMemberNames.IndexOf(x)).ToList();
                GatherPorts();
            }
        }

        //...
        bool DoListCheckout(List<string> options, ref List<string> selected) {
            var changed = false;

            //show potential missing first
            for ( var i = selected.Count; i-- > 0; ) {
                var s = selected[i];
                if ( !options.Contains(s) ) {
                    GUILayout.BeginHorizontal(GUI.skin.box);
                    GUILayout.Label(s.FormatError());
                    if ( GUILayout.Button("Remove", GUILayout.Width(75)) ) {
                        changed = true;
                        UndoUtility.RecordObject(graph, "Extractor");
                        selected.RemoveAt(i);
                        UndoUtility.SetDirty(graph);
                    }
                    GUILayout.EndHorizontal();
                }
            }

            //show expose options
            for ( var i = 0; i < options.Count; i++ ) {
                var name = options[i];
                var active = selected.Contains(name);
                GUI.color = Color.white.WithAlpha(active ? 1 : 0.5f);
                GUILayout.BeginHorizontal(GUI.skin.box);
                GUILayout.Label(name.SplitCamelCase());
                GUILayout.Label("Expose", GUILayout.Width(50));
                GUI.color = Color.white;
                var newActive = UnityEditor.EditorGUILayout.Toggle(active, GUILayout.Width(20));
                GUILayout.EndHorizontal();
                if ( newActive != active ) {
                    changed = true;
                    UndoUtility.RecordObject(graph, "Extractor");
                    if ( newActive == true ) { selected.Add(name); }
                    if ( newActive == false ) { selected.Remove(name); }
                    UndoUtility.SetDirty(graph);
                }
            }
            return changed;
        }

#endif

    }
}