#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using jp.ootr.common.ColorSchema;
using jp.ootr.common.Localization;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using VRC.SDKBase.Editor.BuildPipeline;
using Object = UnityEngine.Object;

namespace jp.ootr.common.Editor
{
    public class BaseEditor : UnityEditor.Editor
    {
        [SerializeField] private StyleSheet styleSheet;
        private bool _debug;
        protected VisualElement InfoBlock;

        protected VisualElement Root;
        protected VisualElement UtilitiesBlock;

        public virtual void OnEnable()
        {
            Root = new VisualElement();
            if (styleSheet != null) Root.styleSheets.Add(styleSheet);
            Root.AddToClassList("root");
            InfoBlock = new VisualElement();
            InfoBlock.AddToClassList("infoBlock");
            UtilitiesBlock = new Foldout
            {
                text = "Utilities",
                value = false
            };
        }

        public override VisualElement CreateInspectorGUI()
        {
            Root.Unbind();
            Root.Clear();
            ShowScriptName();
            Root.Add(ShowLogLevelPicker());

            Root.Add(InfoBlock);

            Root.Add(GetLayout());

            ShowUtilities();

            ShowDebug();
            Root.Bind(serializedObject);
            return Root;
        }

        protected virtual VisualElement GetLayout()
        {
            throw new NotImplementedException();
        }

        private void ShowScriptName()
        {
            var title = new Label(GetScriptName());
            title.AddToClassList("scriptName");
            Root.Add(title);
        }

        protected virtual string GetScriptName()
        {
            return "";
        }

        private VisualElement ShowLogLevelPicker()
        {
            var picker = new EnumField("Log Level", LogLevel.Info)
            {
                label = "Log Level",
                bindingPath = nameof(BaseClass.logLevel)
            };
            return picker;
        }

        private void ShowUtilities()
        {
            UtilitiesBlock.Clear();
            {
                var colorPresetApplier = new Button(() =>
                {
                    var bc = target as BaseClass;
                    if (bc != null)
                        ColorPresetApplier.ShowWindowWithTarget(bc);
                    else
                        Debug.LogWarning("Target is not BaseClass.");
                })
                {
                    text = "ColorPresetApplier"
                };
                UtilitiesBlock.Add(colorPresetApplier);
            }
            Root.Add(UtilitiesBlock);
        }

        private void ShowDebug()
        {
            var foldout = new Foldout
            {
                text = "Debug",
                value = false
            };

            var openLocalizationBtn = new Button(() =>
            {
                var bc = target as BaseClass;
                if (bc != null)
                    LocalizationWindow.ShowWindowWithTarget(bc);
                else
                    Debug.LogWarning("Target is not BaseClass.");
            })
            {
                text = "Open Localization Window"
            };
            foldout.Add(openLocalizationBtn);
            foldout.Add(new IMGUIContainer(base.OnInspectorGUI));
            Root.Add(foldout);
        }
    }

    [InitializeOnLoad]
    public class PlayModeNotifier
    {
        static PlayModeNotifier()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            var classes = ComponentUtils.GetAllComponents<BaseClass>();

            foreach (var c in classes)
            {
                ColorSchemaUtils.ApplyColorSchemas(c);
                LocalizationUtils.SetLocalizationReferences(c);
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.EnteredEditMode) return;
            var classes = ComponentUtils.GetAllComponents<BaseClass>();

            foreach (var c in classes)
            {
                ColorSchemaUtils.ApplyColorSchemas(c);
                LocalizationUtils.SetLocalizationReferences(c);
            }
        }
    }

    public class UnityBuildCallback : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var rootObjects = scene.GetRootGameObjects();

            foreach (var rootObject in rootObjects)
            {
                var baseClasses = rootObject.GetComponentsInChildren<BaseClass>(true);
                foreach (var c in baseClasses)
                {
                    ColorSchemaUtils.ApplyColorSchemas(c);
                    LocalizationUtils.SetLocalizationReferences(c);
                }
            }

            foreach (var rootObject in rootObjects)
            {
                var editorOnlyComponents = rootObject.GetComponentsInChildren<EditorOnlyMonoBehaviour>(true);
                foreach (var component in editorOnlyComponents)
                {
                    if (component == null) continue;
                    Object.DestroyImmediate(component);
                }
            }
        }
    }

    public static class ColorSchemaUtils
    {
        public static void ApplyColorSchemas(BaseClass target)
        {
            if (target == null) return;
            if (target.colorSchemas == null || target.colorSchemaNames == null ||
                target.colorSchemas.Length == 0 || target.colorSchemaNames.Length == 0) return;
            var appliers = target.GetComponentsInChildren<ColorSchemaApplierBase>(true);
            foreach (var applier in appliers)
            {
                if (applier == null) continue;
                applier.ApplyColor(target.GetColor(applier.SchemaName));
            }
        }

        public static Color GetColor(this BaseClass target, string schemaName)
        {
            if (target == null || target.colorSchemas == null || target.colorSchemaNames == null) return Color.white;
            if (string.IsNullOrEmpty(schemaName) || !target.colorSchemaNames.Has(schemaName, out var index)) return Color.white;
            if (index < 0 || index >= target.colorSchemas.Length) return Color.white;
            return target.colorSchemas[index];
        }

        public static BaseClass FindNearestBaseClass(Transform transform)
        {
            if (transform == null) return null;

            var current = transform;
            while (current != null)
            {
                var baseClass = current.GetComponent<BaseClass>();
                if (baseClass != null) return baseClass;

                current = current.parent;
            }

            return null;
        }
    }

    public static class LocalizationUtils
    {
        public static void SetLocalizationReferences(BaseClass target)
        {
            if (target == null) return;
            var targets = target.GetComponentsInChildren<LocalizationApplierTextMeshPro>(true);
            var filteredTargets = new List<LocalizationApplierTextMeshPro>();
            foreach (var t in targets)
            {
                if (t != null) filteredTargets.Add(t);
            }

            var baseClassSo = new SerializedObject(target);
            baseClassSo.Update();
            var localizationTargetKeys = baseClassSo.FindProperty(nameof(BaseClass.localizationTargetKeys));
            var localizationTargets = baseClassSo.FindProperty(nameof(BaseClass.localizationTargets));
            if (localizationTargetKeys == null || localizationTargets == null) return;
            localizationTargetKeys.arraySize = filteredTargets.Count;
            localizationTargets.arraySize = filteredTargets.Count;
            for (var i = 0; i < filteredTargets.Count; i++)
            {
                localizationTargetKeys.GetArrayElementAtIndex(i).stringValue = filteredTargets[i].TextKey;
                localizationTargets.GetArrayElementAtIndex(i).objectReferenceValue = filteredTargets[i].TextMeshProUGUI;
            }

            baseClassSo.ApplyModifiedProperties();
        }

    }
}
#endif
