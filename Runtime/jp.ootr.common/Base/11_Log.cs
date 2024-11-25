using JetBrains.Annotations;
using UnityEngine;

namespace jp.ootr.common.Base
{
    public class BaseClass__Log : BaseClass__ColorSchema
    {
        [SerializeField] internal LogLevel logLevel = LogLevel.Debug;

        public virtual string GetClassName()
        {
            return "jp.ootr.common.BaseClass";
        }

        public virtual string GetDisplayName()
        {
            var names = GetClassName().Split(".");
            return names.Length > 0 ? names[names.Length - 1] : GetClassName();
        }

        protected virtual void ConsoleDebug([CanBeNull]string message, [CanBeNull]string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Debug) Console.Debug(message, GetDisplayName(), prefix);
        }

        protected virtual void ConsoleError([CanBeNull]string message, [CanBeNull]string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Error) Console.Error(message, GetDisplayName(), prefix);
        }

        protected virtual void ConsoleWarn([CanBeNull]string message, [CanBeNull]string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Warn) Console.Warn(message, GetDisplayName(), prefix);
        }

        protected virtual void ConsoleLog([CanBeNull]string message, [CanBeNull]string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Log) Console.Log(message, GetDisplayName(), prefix);
        }

        protected virtual void ConsoleInfo([CanBeNull]string message, [CanBeNull]string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Info) Console.Info(message, GetDisplayName(), prefix);
        }
    }
}
