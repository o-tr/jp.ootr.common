using JetBrains.Annotations;
using VRC.SDKBase;

namespace jp.ootr.common
{
    public enum LogLevel
    {
        Debug,
        Info,
        Log,
        Warn,
        Error
    }

    public static class LogBuilder
    {
        public static string Build(LogLevel level, string message, string packageName = "jp.ootr.common.Console",
            [CanBeNull] string[] prefix = null)
        {
            return $"[<color=lime>{packageName}</color>] {BuildPrefix(prefix)} [{GetLevelString(level)}] {message}";
        }

        private static string GetLevelString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    return "<color=red>Error</color>";
                case LogLevel.Warn:
                    return "<color=yellow>Warn</color>";
                case LogLevel.Log:
                    return "<color=blue>Log</color>";
                case LogLevel.Info:
                    return "<color=green>Info</color>";
                case LogLevel.Debug:
                default:
                    return "<color=gray>Debug</color>";
            }
        }

        private static string BuildPrefix([CanBeNull] string[] prefix)
        {
            if (!Utilities.IsValid(prefix) || prefix.Length == 0) return "";

            return $"[{string.Join("] [", prefix)}]";
        }

        public static string[] CombinePrefix([CanBeNull] string[] prefix, [CanBeNull] string[] additional)
        {
            if (!Utilities.IsValid(prefix) || prefix.Length == 0) return additional;

            if (!Utilities.IsValid(additional) || additional.Length == 0) return prefix;

            return prefix.Merge(additional);
        }
    }
}
