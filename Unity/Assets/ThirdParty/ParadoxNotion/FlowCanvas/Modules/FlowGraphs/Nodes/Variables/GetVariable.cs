using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{
    [Name("Graph Variable", 99)]
    [Category("Variables/New")]
    [Description("Returns a constant or linked variable value.\nYou can alter between constant or linked at any time using the radio button.")]
    [ContextDefinedOutputs(typeof(Wild))]
    public class GetVariable<T> : ParameterVariableNode
    {

        public BBParameter<T> value;
        public override BBParameter parameter => value;

#if UNITY_EDITOR
        public override string name {
            get
            {
                var size = typeof(T).IsPrimitive && !value.useBlackboard ? 20 : 12;
                return string.Format("<size={0}>{1}</size>", size.ToString(), value.ToString());
            }
        }
#endif

        protected override void RegisterPorts() {
            AddValueOutput<T>("Value", () => { return value.value; });
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnNodeGUI() {
            base.OnNodeGUI();

            if ( verboseLevel != Node.VerboseLevel.Full ) {
                return;
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            T theValue = value.value;
            if ( theValue is UnityEngine.Object && !theValue.Equals(null) ) {
                var o = (UnityEngine.Object)(object)theValue;

#if !UNITY_2018_3_OR_NEWER //works in 2018.3 without this code
                {
                    var prefabType = UnityEditor.PrefabUtility.GetPrefabType(o);
                    if ( prefabType == UnityEditor.PrefabType.PrefabInstance || prefabType == UnityEditor.PrefabType.PrefabInstance ) {
#if UNITY_2018_2_OR_NEWER
						o = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(o);
#else
                        o = UnityEditor.PrefabUtility.GetPrefabParent(o);
#endif
                    }
                }
#endif
                if ( o is Component ) { o = ( o as Component ).gameObject; }
                var texture = UnityEditor.AssetPreview.GetAssetPreview(o);
                GUI.backgroundColor = ColorUtils.Grey(UnityEditor.EditorGUIUtility.isProSkin ? 0.15f : 0.6f);
                GUILayout.Box(texture != null ? texture : Texture2D.whiteTexture, GUILayout.Width(64), GUILayout.Height(64));
                if ( texture == null ) { GUI.Label(GUILayoutUtility.GetLastRect(), "<size=8>NO PREVIEW\nPOSSIBLE</size>"); }
                GUI.backgroundColor = Color.white;
            }

            if ( theValue is Color ) {
                var color = (Color)(object)theValue;
                GUI.color = color;
                GUILayout.Box(string.Empty, GUILayout.Width(32), GUILayout.Height(32));
                var lastRect = GUILayoutUtility.GetLastRect();
                GUI.DrawTexture(lastRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

#endif

    }
}