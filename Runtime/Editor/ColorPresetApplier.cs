using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR

namespace jp.ootr.common.Editor
{
    public class ColorPresetApplier : EditorWindow
    {
        private BaseClass _target;
        private ObjectField _targetField;
        
        public void CreateGUI()
        {
            var root = rootVisualElement;
            
            root.Add(GetTargetPicker());
            
            root.Add(GetColorPresetPicker());
        }
        
        [MenuItem("Tools/ootr/ColorPresetApplier")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<ColorPresetApplier>();
            wnd.titleContent = new GUIContent("ColorPresetApplier");
        }
        
        public static void ShowWindowWithTarget(BaseClass target)
        {
            var wnd = GetWindow<ColorPresetApplier>();
            wnd.titleContent = new GUIContent("ColorPresetApplier");
            wnd._target = target;
            wnd._targetField.value = target;
        }
        
        private VisualElement GetTargetPicker()
        {
            var root = new VisualElement();
            _targetField = new ObjectField
            {
                label = "Target",
                objectType = typeof(BaseClass),
                value = _target
            };
            _targetField.RegisterValueChangedCallback(evt =>
            {
                _target = (BaseClass)evt.newValue;
            });
            root.Add(_targetField);
            return root;
        }
        
        private VisualElement GetColorPresetPicker()
        {
            var root = new VisualElement();
            var presetField = new ObjectField
            {
                label = "Color Preset",
                objectType = typeof(ColorPreset),
            };
            presetField.RegisterValueChangedCallback(evt =>
            {
                var preset = (ColorPreset)evt.newValue;
                if (preset == null) return;
                if (_target == null) return;
                ApplyColorPreset(preset);
            });
            root.Add(presetField);
            
            var applyButton = new Button { text = "Apply" };
            applyButton.SetEnabled(false);
            presetField.RegisterValueChangedCallback(evt =>
            {
                applyButton.SetEnabled(evt.newValue != null && _target != null);
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
        
        private void ApplyColorPreset(ColorPreset preset)
        {
            var so = new SerializedObject(_target);
            var colors = so.FindProperty(nameof(BaseClass.colorSchemes));
            var names = so.FindProperty(nameof(BaseClass.colorSchemeNames));
            colors.arraySize = preset.colors.Length;
            names.arraySize = preset.names.Length;
            for (var i = 0; i < preset.colors.Length; i++)
            {
                colors.GetArrayElementAtIndex(i).colorValue = preset.colors[i];
                names.GetArrayElementAtIndex(i).stringValue = preset.names[i];
            }
            so.ApplyModifiedProperties();
            BaseClassUtils.ApplyColorSchemas(_target);
        }
    }
}
#endif
