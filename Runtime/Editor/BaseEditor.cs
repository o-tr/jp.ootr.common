#if UNITY_EDITOR
using jp.ootr.common.ColorSchema;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDKBase.Editor.BuildPipeline;

namespace jp.ootr.common.Editor
{
    public class BaseEditor : UnityEditor.Editor
    {
        private bool _debug;

        protected VisualElement Root;
        protected VisualElement InfoBlock;
        
        [SerializeField] private StyleSheet styleSheet;
        
        public virtual void OnEnable()
        {
            Root = new VisualElement();
            Root.styleSheets.Add(styleSheet);
            Root.AddToClassList("root");
            InfoBlock = new VisualElement();
            InfoBlock.AddToClassList("infoBlock");
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            Root.Clear();
            ShowScriptName();
            
            Root.Add(InfoBlock);
            
            Root.Add(GetLayout());
            
            ShowDebug();
            return Root;
        }
        
        protected virtual VisualElement GetLayout()
        {
            throw new System.NotImplementedException();
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

        private void ShowDebug()
        {
            var foldout = new Foldout()
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
            
            foreach (var c in classes) BaseClassUtils.ApplyColorSchemas(c);
        }
        
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            var classes = ComponentUtils.GetAllComponents<BaseClass>();
            
            foreach (var c in classes) BaseClassUtils.ApplyColorSchemas(c);
        }
    }

    public class BuildCallback : UnityEditor.Editor, IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => 0;
        
        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            var classes = ComponentUtils.GetAllComponents<BaseClass>();
            
            foreach (var c in classes) BaseClassUtils.ApplyColorSchemas(c);
            
            return true;
        }
    }

    public class UnityBuildCallback : IProcessSceneWithReport
    {
        public int callbackOrder => 0;
        
        public void OnProcessScene(UnityEngine.SceneManagement.Scene scene, BuildReport report)
        {
            var classes = ComponentUtils.GetAllComponents<BaseClass>();
            
            foreach (var c in classes) BaseClassUtils.DestroyColorAppliers(c);
        }
    }

    public static class BaseClassUtils
    {
        public static void ApplyColorSchemas(BaseClass target)
        {
            var appliers = target.GetComponentsInChildren<ColorSchemaApplierBase>(true);
            foreach (var applier in appliers)
            {
                applier.ApplyColor(target.GetColor(applier.SchemaName));
            }
        }
        
        public static void DestroyColorAppliers(BaseClass target)
        {
            var appliers = target.GetComponentsInChildren<ColorSchemaApplierBase>(true);
            foreach (var applier in appliers)
            {
                Object.DestroyImmediate(applier);
            }
        }

        public static Color GetColor(this BaseClass target, string schemaName)
        {
            if (target == null) return Color.white;
            if (!target.colorSchemeNames.Has(schemaName, out var index)) return Color.white;
            return target.colorSchemes[index];
        }
    }
}
#endif
