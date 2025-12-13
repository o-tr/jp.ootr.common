#if UNITY_EDITOR
using jp.ootr.common.ColorSchema;
using UnityEditor;

namespace jp.ootr.common.Editor
{
    [InitializeOnLoad]
    internal class GizmoIcons
    {
        private static bool _gizmosDisabled = false;

        static GizmoIcons()
        {
            EditorApplication.update += DisableGizmos;
        }

        internal static void DisableGizmos()
        {
            if (_gizmosDisabled) return;
            
            GizmoUtility.SetIconEnabled(typeof(ColorSchemaApplierImage), false);
            GizmoUtility.SetIconEnabled(typeof(ColorSchemaApplierText), false);
            GizmoUtility.SetIconEnabled(typeof(ColorSchemaApplierRawImage), false);
            GizmoUtility.SetIconEnabled(typeof(ColorSchemaApplierTextMeshProUGUI), false);
            EditorApplication.update -= DisableGizmos;
            
            _gizmosDisabled = true;
        }
    }
}
#endif
