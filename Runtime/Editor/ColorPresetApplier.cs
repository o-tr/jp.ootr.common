#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.ootr.common.Editor
{
    public class ColorPresetApplier : EditorWindow
    {
        [SerializeField] private StyleSheet baseStyleSheet;
        private BaseClass _target;
        private ObjectField _targetField;

        public void CreateGUI()
        {
            var root = new VisualElement();
            root.styleSheets.Add(baseStyleSheet);
            root.AddToClassList("root");

            root.Add(GetTargetPicker());

            root.Add(GetColorPresetPicker());
            rootVisualElement.Add(root);
        }

        [MenuItem("Tools/ootr/ColorPresetApplier")]
        public static void ShowWindow()
        {
            GetWindow();
        }

        public static void ShowWindowWithTarget(BaseClass target)
        {
            var wnd = GetWindow();
            wnd._target = target;
            wnd._targetField.value = target;
        }

        private static ColorPresetApplier GetWindow()
        {
            var window = GetWindow<ColorPresetApplier>();
            window.titleContent = new GUIContent("ColorPresetApplier");
            return window;
        }

        private VisualElement GetTargetPicker()
        {
            var root = new VisualElement();
            var preview = new VisualElement();
            _targetField = new ObjectField
            {
                label = "Target",
                objectType = typeof(BaseClass),
                value = _target
            };
            _targetField.RegisterValueChangedCallback(evt =>
            {
                _target = (BaseClass)evt.newValue;
                if (_target == null) return;
                preview.Clear();
                preview.Add(GeneratePreview(_target));
            });
            root.Add(_targetField);
            root.Add(preview);
            if (_target != null) preview.Add(GeneratePreview(_target));
            return root;
        }

        private VisualElement GetColorPresetPicker()
        {
            var root = new VisualElement();
            var presetField = new ObjectField
            {
                label = "Color Preset",
                objectType = typeof(ColorPreset)
            };
            var preview = new VisualElement();
            presetField.RegisterValueChangedCallback(evt =>
            {
                var preset = (ColorPreset)evt.newValue;
                if (preset == null) return;
                if (_target == null) return;
                ApplyColorPreset(preset);
            });
            root.Add(presetField);
            root.Add(preview);

            var applyButton = new Button { text = "Apply" };
            applyButton.SetEnabled(false);
            presetField.RegisterValueChangedCallback(evt =>
            {
                applyButton.SetEnabled(evt.newValue != null && _target != null);
                preview.Clear();
                if (evt.newValue != null)
                {
                    var preset = (ColorPreset)evt.newValue;
                    preview.Add(GeneratePreview(preset.names, preset.colors));
                }
            });
            _targetField.RegisterValueChangedCallback(evt =>
            {
                applyButton.SetEnabled(evt.newValue != null && presetField.value != null);
            });
            applyButton.clicked += () =>
            {
                var preset = (ColorPreset)presetField.value;
                ApplyColorPreset(preset);
            };
            root.Add(applyButton);

            return root;
        }

        private VisualElement GeneratePreview(BaseClass baseClass)
        {
            var so = new SerializedObject(baseClass);
            var colors = so.FindProperty(nameof(BaseClass.colorSchemas));
            var names = so.FindProperty(nameof(BaseClass.colorSchemaNames));
            var schemaName = new string[colors.arraySize];
            var color = new Color[colors.arraySize];
            for (var i = 0; i < colors.arraySize; i++)
            {
                schemaName[i] = names.GetArrayElementAtIndex(i).stringValue;
                color[i] = colors.GetArrayElementAtIndex(i).colorValue;
            }

            return GeneratePreview(schemaName, color);
        }

        private VisualElement GeneratePreview(string[] schemaName, Color[] colors)
        {
            var root = new VisualElement();
            for (var i = 0; i < schemaName.Length; i++)
            {
                var row = new VisualElement();
                row.AddToClassList("row");
                var label = new Label(schemaName[i]);
                var colorBox = new VisualElement();
                colorBox.style.backgroundColor = new StyleColor(colors[i]);
                colorBox.style.width = 20;
                colorBox.style.height = 20;
                row.Add(label);
                row.Add(colorBox);
                root.Add(row);
            }

            return root;
        }

        private void ApplyColorPreset(ColorPreset preset)
        {
            var so = new SerializedObject(_target);
            var colors = so.FindProperty(nameof(BaseClass.colorSchemas));
            var names = so.FindProperty(nameof(BaseClass.colorSchemaNames));
            colors.arraySize = preset.colors.Length;
            names.arraySize = preset.names.Length;
            for (var i = 0; i < preset.colors.Length; i++)
            {
                colors.GetArrayElementAtIndex(i).colorValue = preset.colors[i];
                names.GetArrayElementAtIndex(i).stringValue = preset.names[i];
            }

            so.ApplyModifiedProperties();
            ColorSchemaUtils.ApplyColorSchemas(_target);
        }
    }
}
#endif
