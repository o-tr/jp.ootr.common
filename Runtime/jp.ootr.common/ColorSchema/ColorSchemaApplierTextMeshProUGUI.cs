#if UNITY_EDITOR
using TMPro;
using UnityEngine;
using UnityEditor;

namespace jp.ootr.common.ColorSchema
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ColorSchemaApplierTextMeshProUGUI : ColorSchemaApplierBase
    {
        public override void ApplyColor(Color color)
        {
            var text = gameObject.GetComponent<TextMeshProUGUI>();
            if (text == null) return;
            var so = new SerializedObject(text);
            so.Update();
            so.FindProperty("m_fontColor").colorValue = color;
            so.FindProperty("m_faceColor").colorValue = color;
            so.ApplyModifiedProperties();
        }
    }
}
#endif
