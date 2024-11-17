using jp.ootr.common.Base;

namespace jp.ootr.common
{
    public class BaseClass : BaseClass__CommonBase
    {
        protected readonly int SyncURLRetryCountLimit = 3;
        protected readonly float SyncURLRetryInterval = 0.5f;
    }
}
