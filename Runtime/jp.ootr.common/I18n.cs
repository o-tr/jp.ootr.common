using VRC.SDKBase;

namespace jp.ootr.common
{
    public static class I18n
    {
        public static Language GetSystemLanguage()
        {
            switch (VRCPlayerApi.GetCurrentLanguage())
            {
                case "Japanese":
                    return Language.JaJp;
                default:
                    return Language.EnUs;
            }
        }
    }

    public enum Language
    {
        JaJp,
        EnUs
    }
}
