#if UNITY_EDITOR
using jp.ootr.common.ColorSchema;
using UnityEditor;

namespace jp.ootr.common.Editor
{
    [InitializeOnLoad]
    internal class GizmoIcons
    {
        static GizmoIcons()
        {
            EditorApplication.update += DisableGizmos;
        }

        internal static void DisableGizmos()
        {
            GizmoUtility.SetIconEnabled(typeof(ColorSchemaApplierImage), false);
            GizmoUtility.SetIconEnabled(typeof(ColorSchemaApplierText), false);
            GizmoUtility.SetIconEnabled(typeof(ColorSchemaApplierRawImage), false);
            GizmoUtility.SetIconEnabled(typeof(ColorSchemaApplierTextMeshProUGUI), false);
            EditorApplication.update -= DisableGizmos;
        }
    }
}
#endif
