#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace jp.ootr.common.ColorSchema
{
    [RequireComponent(typeof(Text))]
    public class ColorSchemaApplierText : ColorSchemaApplierBase
    {
        public override void ApplyColor(Color color)
        {
            var text = gameObject.GetComponent<Text>();
            if (text == null) return;
            var so = new SerializedObject(text);
            so.Update();
            so.FindProperty("m_Color").colorValue = color;
            so.ApplyModifiedProperties();
        }
    }
}
#endif
