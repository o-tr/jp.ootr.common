using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace jp.ootr.common.ColorSchema
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ColorSchemaApplierTextMeshProUGUI : ColorSchemaApplierBase
    {
        public override void ApplyColor(Color color)
        {
            var text = gameObject.GetComponent<TextMeshProUGUI>();
            if (text == null) return;
#if UNITY_EDITOR
            var so = new SerializedObject(text);
            so.Update();
            so.FindProperty("m_fontColor").colorValue = color;
            so.FindProperty("m_faceColor").colorValue = color;
            so.ApplyModifiedProperties();
#else
            text.color = color;
#endif
        }
    }
}
