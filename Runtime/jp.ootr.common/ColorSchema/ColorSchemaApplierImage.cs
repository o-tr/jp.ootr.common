#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace jp.ootr.common.ColorSchema
{
    [RequireComponent(typeof(Image))]
    public class ColorSchemaApplierImage : ColorSchemaApplierBase
    {
        public override void ApplyColor(Color color)
        {
            var image = gameObject.GetComponent<Image>();
            if (image == null) return;
#if UNITY_EDITOR
            var so = new SerializedObject(image);
            so.Update();
            so.FindProperty("m_Color").colorValue = color;
            so.ApplyModifiedProperties();
#else
            image.color = color;
#endif
        }
    }
}
