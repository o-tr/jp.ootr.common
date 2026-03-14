#if UNITY_EDITOR
using System.Collections.Generic;
using jp.ootr.common.Base;
using jp.ootr.common.Localization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.ootr.common.Editor
{
    [CustomEditor(typeof(LocalizationApplierTextMeshPro), true)]
    public class LocalizationApplierEditor : UnityEditor.Editor
    {
        private SerializedProperty _textKeyProperty;
        private BaseClass _baseClass;
        private List<string> _logicalKeys;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            _textKeyProperty = serializedObject.FindProperty("textKey");
            if (_textKeyProperty == null)
            {
                root.Add(new Label("Error: textKey property not found"));
                return root;
            }

            _baseClass = ColorSchemaUtils.FindNearestBaseClass((target as MonoBehaviour)?.transform);
            UpdateLogicalKeys();

            if (_baseClass != null && _logicalKeys != null && _logicalKeys.Count > 0)
            {
                root.Add(CreateDropdownField());
            }
            else
            {
                root.Add(CreateTextField());
            }

            return root;
        }

        private void UpdateLogicalKeys()
        {
            _logicalKeys = null;
            if (_baseClass == null) return;

            var so = new SerializedObject(_baseClass);
            so.Update();

            var keysProp = so.FindProperty("localizationKeys");
            if (keysProp == null || keysProp.arraySize == 0) return;

            var seen = new HashSet<string>();
            _logicalKeys = new List<string>();

            for (var i = 0; i < keysProp.arraySize; i++)
            {
                var fullKey = keysProp.GetArrayElementAtIndex(i).stringValue;
                if (string.IsNullOrEmpty(fullKey)) continue;

                var dot = fullKey.IndexOf('.');
                if (dot < 0) continue;

                var logicalKey = fullKey.Substring(dot + 1);
                if (seen.Add(logicalKey))
                    _logicalKeys.Add(logicalKey);
            }
        }

        private VisualElement CreateDropdownField()
        {
            var currentKey = _textKeyProperty.stringValue ?? "";
            var choices = new List<string>(_logicalKeys);

            if (!string.IsNullOrEmpty(currentKey) && !choices.Contains(currentKey))
                choices.Insert(0, currentKey);

            var defaultIndex = choices.IndexOf(currentKey);
            if (defaultIndex < 0)
                defaultIndex = 0;

            var dropdown = new DropdownField("Text Key", choices, defaultIndex);
            dropdown.RegisterValueChangedCallback(evt =>
            {
                _textKeyProperty.stringValue = evt.newValue;
                serializedObject.ApplyModifiedProperties();
            });

            return dropdown;
        }

        private VisualElement CreateTextField()
        {
            var textField = new TextField("Text Key")
            {
                bindingPath = "textKey"
            };
            textField.Bind(serializedObject);
            return textField;
        }
    }
}
#endif
