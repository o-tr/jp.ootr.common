#if UNITY_EDITOR
using System;
using jp.ootr.common.ColorSchema;
using jp.ootr.common.Localization;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
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
            Root.styleSheets.Add(styleSheet);
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
            Root.Clear();
            ShowScriptName();
            Root.Add(ShowLogLevelPicker());

            Root.Add(InfoBlock);

            Root.Add(GetLayout());

            ShowUtilities();

            ShowDebug();
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
            {
                var colorPresetApplier = new Button(() =>
                {
                    ColorPresetApplier.ShowWindowWithTarget(target as BaseClass);
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

            foldout.Add(new IMGUIContainer(base.OnInspectorGUI));
            Root.Add(foldout);
        }
    }

    [InitializeOnLoad]
    public class PlayModeNotifier
    {
        static PlayModeNotifier()
        {
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
            var classes = ComponentUtils.GetAllComponents<BaseClass>();

            foreach (var c in classes)
            {
                ColorSchemaUtils.ApplyColorSchemas(c);
                LocalizationUtils.SetLocalizationReferences(c);
            }
        }
    }

    public class BuildCallback : UnityEditor.Editor, IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => 0;

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            var classes = ComponentUtils.GetAllComponents<BaseClass>();

            foreach (var c in classes)
            {
                ColorSchemaUtils.ApplyColorSchemas(c);
                LocalizationUtils.SetLocalizationReferences(c);
            }

            return true;
        }
    }

    public class UnityBuildCallback : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var classes = ComponentUtils.GetAllComponents<BaseClass>();

            foreach (var c in classes)
            {
                ColorSchemaUtils.DestroyColorAppliers(c);
                LocalizationUtils.DestroyLocalizations(c);
            }
        }
    }

    public static class ColorSchemaUtils
    {
        public static void ApplyColorSchemas(BaseClass target)
        {
            if (target.colorSchemas.Length == 0 || target.colorSchemaNames.Length == 0) return;
            var appliers = target.GetComponentsInChildren<ColorSchemaApplierBase>(true);
            foreach (var applier in appliers) applier.ApplyColor(target.GetColor(applier.SchemaName));
        }

        public static void DestroyColorAppliers(BaseClass target)
        {
            var appliers = target.GetComponentsInChildren<ColorSchemaApplierBase>(true);
            foreach (var applier in appliers) Object.DestroyImmediate(applier);
        }

        public static Color GetColor(this BaseClass target, string schemaName)
        {
            if (target == null) return Color.white;
            if (!target.colorSchemaNames.Has(schemaName, out var index)) return Color.white;
            return target.colorSchemas[index];
        }
    }

    public static class LocalizationUtils
    {
        public static void SetLocalizationReferences(BaseClass target)
        {
            var targets = target.GetComponentsInChildren<LocalizationApplierTextMeshPro>(true);
            var baseClassSo = new SerializedObject(target);
            baseClassSo.Update();
            var localizationTargetKeys = baseClassSo.FindProperty(nameof(BaseClass.localizationTargetKeys));
            var localizationTargets = baseClassSo.FindProperty(nameof(BaseClass.localizationTargets));
            localizationTargetKeys.arraySize = targets.Length;
            localizationTargets.arraySize = targets.Length;
            for (var i = 0; i < targets.Length; i++)
            {
                localizationTargetKeys.GetArrayElementAtIndex(i).stringValue = targets[i].TextKey;
                localizationTargets.GetArrayElementAtIndex(i).objectReferenceValue = targets[i].TextMeshProUGUI;
            }

            baseClassSo.ApplyModifiedProperties();
        }

        public static void DestroyLocalizations(BaseClass target)
        {
            var appliers = target.GetComponentsInChildren<LocalizationApplierTextMeshPro>(true);
            foreach (var applier in appliers) Object.DestroyImmediate(applier);
        }
    }
}
#endif
