using JetBrains.Annotations;

namespace jp.ootr.common
{
    public static class Console
    {
        public const string PackageName = "jp.ootr.common.Console";

        public static void Error([CanBeNull] string message, string packageName = PackageName, [CanBeNull] string[] prefix = null)
        {
            UnityEngine.Debug.LogError(LogBuilder.Build(LogLevel.Error, message, packageName, prefix));
        }

        public static void Warn([CanBeNull] string message, string packageName = PackageName, [CanBeNull] string[] prefix = null)
        {
            UnityEngine.Debug.LogWarning(LogBuilder.Build(LogLevel.Warn, message, packageName, prefix));
        }

        public static void Log([CanBeNull] string message, string packageName = PackageName, [CanBeNull] string[] prefix = null)
        {
            UnityEngine.Debug.Log(LogBuilder.Build(LogLevel.Log, message, packageName, prefix));
        }

        public static void Info([CanBeNull] string message, string packageName = PackageName, [CanBeNull] string[] prefix = null)
        {
            UnityEngine.Debug.Log(LogBuilder.Build(LogLevel.Info, message, packageName, prefix));
        }

        public static void Debug([CanBeNull] string message, string packageName = PackageName, [CanBeNull] string[] prefix = null)
        {
            UnityEngine.Debug.Log(LogBuilder.Build(LogLevel.Debug, message, packageName, prefix));
        }
    }
}
