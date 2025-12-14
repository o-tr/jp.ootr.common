#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace jp.ootr.common.ColorSchema
{
    [RequireComponent(typeof(RawImage))]
    public class ColorSchemaApplierRawImage : ColorSchemaApplierBase
    {
        public override void ApplyColor(Color color)
        {
            var image = gameObject.GetComponent<RawImage>();
            if (image == null) return;
            var so = new SerializedObject(image);
            so.Update();
            so.FindProperty("m_Color").colorValue = color;
            so.ApplyModifiedProperties();
        }
    }
}
#endif
