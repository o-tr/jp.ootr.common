using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace jp.ootr.common.ColorSchema
{
    [RequireComponent(typeof(Text))]
    public class ColorSchemaApplierText : ColorSchemaApplierBase
    {
        public override void ApplyColor(Color color)
        {
            var text = gameObject.GetComponent<Text>();
            if (text == null) return;
#if UNITY_EDITOR
            var so = new SerializedObject(text);
            so.Update();
            so.FindProperty("m_Color").colorValue = color;
            so.ApplyModifiedProperties();
#else
            text.color = color;
#endif
        }
    }
}
