using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common;
using VRC.Udon.Common.Enums;

namespace jp.ootr.common
{
    public class BaseClass : UdonSharpBehaviour
    {
        [SerializeField] internal LogLevel logLevel = LogLevel.Debug;
        protected readonly int SyncURLRetryCountLimit = 3;
        protected readonly float SyncURLRetryInterval = 0.5f;
        
        [SerializeField] internal string[] colorSchemeNames;
        [SerializeField] internal Color[] colorSchemes;

        public virtual string GetClassName()
        {
            return "jp.ootr.common.BaseClass";
        }

        public virtual string GetDisplayName()
        {
            var names = GetClassName().Split(".");
            return names.Length > 0 ? names[names.Length - 1] : GetClassName();
        }

        protected virtual void ConsoleDebug(string message, string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Debug) Console.Debug(message, GetDisplayName(), prefix);
        }

        protected virtual void ConsoleError(string message, string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Error) Console.Error(message, GetDisplayName(), prefix);
        }

        protected virtual void ConsoleWarn(string message, string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Warn) Console.Warn(message, GetDisplayName(), prefix);
        }

        protected virtual void ConsoleLog(string message, string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Log) Console.Log(message, GetDisplayName(), prefix);
        }

        protected virtual void ConsoleInfo(string message, string[] prefix = null)
        {
            if ((int)logLevel <= (int)LogLevel.Info) Console.Info(message, GetDisplayName(), prefix);
        }

        protected virtual void Sync()
        {
            if (!Networking.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            RequestSerialization();
            //HACK: UnityのプレイモードではOnPostSerializationが発火しないため手動で呼び出す必要がある
#if UNITY_EDITOR
            SendCustomEventDelayedFrames(nameof(_OnDeserialization), 5, EventTiming.LateUpdate);
#endif
        }

        public override void OnPostSerialization(SerializationResult result)
        {
            SendCustomEventDelayedFrames(nameof(_OnDeserialization), 1, EventTiming.LateUpdate);
        }

        public override void OnDeserialization()
        {
            _OnDeserialization();
        }

        public virtual void _OnDeserialization()
        {
        }
    }
}
