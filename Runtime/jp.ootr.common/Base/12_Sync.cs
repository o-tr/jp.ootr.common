using VRC.SDKBase;
using VRC.Udon.Common;
using VRC.Udon.Common.Enums;

namespace jp.ootr.common.Base
{
    public class BaseClass__Sync : BaseClass__Log {
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