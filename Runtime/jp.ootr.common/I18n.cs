using System;
using VRC.SDKBase;

namespace jp.ootr.common
{
    [Obsolete("Use jp.ootr.common.Localization.LanguageUtils instead")]
    public static class I18n
    {
        [Obsolete("Use jp.ootr.common.Localization.LanguageUtils.GetCurrentLanguage instead")]
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

    [Obsolete("Use jp.ootr.common.Localization.Language instead")]
    public enum Language
    {
        JaJp,
        EnUs
    }
}
