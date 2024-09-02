#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace jp.ootr.common.Editor
{
    public class LogLevelSetter : EditorWindow
    {
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Set log level for all ootr's objects in current scene.");

            var logLevel = (LogLevel)EditorPrefs.GetInt("jp.ootr.common.LogLevel", (int)LogLevel.Debug);
            var newLogLevel = (LogLevel)EditorGUILayout.EnumPopup("Log Level", logLevel);
            if (newLogLevel != logLevel) EditorPrefs.SetInt("jp.ootr.common.LogLevel", (int)newLogLevel);

            if (GUILayout.Button("Apply")) ApplyLogLevel(newLogLevel);
        }

        [MenuItem("Tools/ootr/Log Level Setter")]
        private static void Create()
        {
            GetWindow<LogLevelSetter>("Log Level Setter");
        }


        private void ApplyLogLevel(LogLevel level)
        {
            var objects = FindObjectsOfType<BaseClass>();
            foreach (var obj in objects)
            {
                var so = new SerializedObject(obj);
                so.FindProperty("logLevel").enumValueIndex = (int)level;
                so.ApplyModifiedProperties();
            }
        }
    }
}
#endif
