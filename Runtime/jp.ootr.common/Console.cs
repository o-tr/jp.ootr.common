namespace jp.ootr.common
{
    public static class Console
    {
        public const string PackageName = "jp.ootr.common.Console";

        public static void Error(string message, string packageName = PackageName, string prefix = "")
        {
            UnityEngine.Debug.LogError(
                $"[<color=lime>{packageName}</color>]{prefix} [<color=red>Error</color>] {message}");
        }

        public static void Warn(string message, string packageName = PackageName, string prefix = "")
        {
            UnityEngine.Debug.LogWarning(
                $"[<color=lime>{packageName}</color>]{prefix} [<color=yellow>Warn</color>] {message}");
        }

        public static void Log(string message, string packageName = PackageName, string prefix = "")
        {
            UnityEngine.Debug.Log($"[<color=lime>{packageName}</color>]{prefix} [<color=blue>Log</color>] {message}");
        }

        public static void Info(string message, string packageName = PackageName, string prefix = "")
        {
            UnityEngine.Debug.Log($"[<color=lime>{packageName}</color>]{prefix} [<color=green>Info</color>] {message}");
        }

        public static void Debug(string message, string packageName = PackageName, string prefix = "")
        {
            UnityEngine.Debug.Log($"[<color=lime>{packageName}</color>]{prefix} [<color=gray>Debug</color>] {message}");
        }
    }
}