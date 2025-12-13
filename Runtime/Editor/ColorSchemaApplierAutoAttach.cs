#if UNITY_EDITOR
using jp.ootr.common;
using jp.ootr.common.Base;
using jp.ootr.common.ColorSchema;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace jp.ootr.common.Editor
{
    [InitializeOnLoad]
    public static class ColorSchemaApplierAutoAttach
    {
        static ColorSchemaApplierAutoAttach()
        {
            ObjectFactory.componentWasAdded += OnComponentAdded;
        }

        private static void OnComponentAdded(Component component)
        {
            if (component == null) return;

            var gameObject = component.gameObject;
            if (gameObject == null) return;

            System.Type applierType = null;

            if (component is Image)
            {
                applierType = typeof(ColorSchemaApplierImage);
            }
            else if (component is RawImage)
            {
                applierType = typeof(ColorSchemaApplierRawImage);
            }
            else if (component is Text)
            {
                applierType = typeof(ColorSchemaApplierText);
            }
            else if (component is TextMeshProUGUI)
            {
                applierType = typeof(ColorSchemaApplierTextMeshProUGUI);
            }

            if (applierType == null) return;

            var baseClass = ColorSchemaUtils.FindNearestBaseClass(gameObject.transform);
            if (baseClass == null) return;

            if (gameObject.GetComponent(applierType) != null) return;

            try
            {
                Undo.AddComponent(gameObject, applierType);
                EditorUtility.SetDirty(gameObject);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Failed to add ColorSchemaApplier to {gameObject.name}: {ex.Message}");
            }
        }
    }
}
#endif

