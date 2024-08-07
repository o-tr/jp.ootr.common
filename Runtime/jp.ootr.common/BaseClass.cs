using UdonSharp;
using VRC.SDKBase;
using VRC.Udon.Common;
using VRC.Udon.Common.Enums;

namespace jp.ootr.common
{
    public class BaseClass : UdonSharpBehaviour
    {
        protected readonly int SyncURLRetryCountLimit = 3;
        protected readonly float SyncURLRetryInterval = 0.5f;

        public virtual string GetClassName()
        {
            return "jp.ootr.common.BaseClass";
        }

        public virtual string GetDisplayName()
        {
            return GetClassName();
        }

        protected virtual void ConsoleDebug(string message, string prefix = "")
        {
            Console.Debug(message, GetClassName(), prefix);
        }

        protected virtual void ConsoleError(string message, string prefix = "")
        {
            Console.Error(message, GetClassName(), prefix);
        }

        protected virtual void ConsoleWarn(string message, string prefix = "")
        {
            Console.Warn(message, GetClassName(), prefix);
        }

        protected virtual void ConsoleLog(string message, string prefix = "")
        {
            Console.Log(message, GetClassName(), prefix);
        }

        protected virtual void ConsoleInfo(string message, string prefix = "")
        {
            Console.Info(message, GetClassName(), prefix);
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