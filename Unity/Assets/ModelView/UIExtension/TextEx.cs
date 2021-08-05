using ET;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/TextEx", 10)]
public class TextEx : Text
{
    public int localizationTextId = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            var textAsset = AssetDatabase.LoadAssetAtPath<LocalizationTextAsset>
                    ("Assets/EditorRes/LocalizationTextAsset.asset");
            
            if (textAsset.Default.TryGetValue(localizationTextId, out var text))
            {
                this.text = text;
            }
            else
            {
                Debug.LogError($"多语言文本配置错误 Id：{localizationTextId.ToString()}");
                this.text = "Null";
            }
        }
#endif
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            this.text = null;
        }
#endif
    }
}
