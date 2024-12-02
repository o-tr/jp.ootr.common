namespace jp.ootr.common.Base
{
    public class BaseClass__Platform : BaseClass__Localization {
#if UNITY_ANDROID
        public const Platform CurrentPlatform = Platform.Android;
#elif UNITY_IOS 
        public const Platform CurrentPlatform = Platform.iOS;
#else
        public const Platform CurrentPlatform = Platform.PC;
#endif
    }

    public enum Platform
    {
        PC,
        Android,
        iOS,
    }
}
