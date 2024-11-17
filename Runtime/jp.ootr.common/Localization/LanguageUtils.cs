using UnityEngine;
using VRC.SDKBase;

namespace jp.ootr.common.Localization
{
    public static class LanguageUtils
    {
        public static Language GetCurrentLanguage()
        {
            var langStr = VRCPlayerApi.GetCurrentLanguage();
            switch (langStr)
            {
                case "en":
                    return Language.En;
                case "fr":
                    return Language.Fr;
                case "es":
                    return Language.ES;
                case "it":
                    return Language.It;
                case "ko":
                    return Language.Ko;
                case "de":
                    return Language.De;
                case "ja":
                    return Language.Ja;
                case "pl":
                    return Language.Pl;
                case "ru":
                    return Language.Ru;
                case "pt_BR":
                    return Language.PtBR;
                case "zh_CN":
                    return Language.ZhCn;
                case "zh_HK":
                    return Language.ZhHk;
                case "he":
                    return Language.He;
                case "tok":
                    return Language.Tok;
                case "uk":
                    return Language.Uk;
                default:
                    Debug.LogWarning($"Unsupported language: {langStr}, fallback to en");
                    return Language.En;
            }
        }
        
        public static string ToStr(this Language lang)
        {
            switch (lang)
            {
                case Language.En:
                    return "en";
                case Language.Fr:
                    return "fr";
                case Language.ES:
                    return "es";
                case Language.It:
                    return "it";
                case Language.Ko:
                    return "ko";
                case Language.De:
                    return "de";
                case Language.Ja:
                    return "ja";
                case Language.Pl:
                    return "pl";
                case Language.Ru:
                    return "ru";
                case Language.PtBR:
                    return "pt_BR";
                case Language.ZhCn:
                    return "zh_CN";
                case Language.ZhHk:
                    return "zh_HK";
                case Language.He:
                    return "he";
                case Language.Tok:
                    return "tok";
                case Language.Uk:
                    return "uk";
                default:
                    return "en";
            }
        }
    }
}
