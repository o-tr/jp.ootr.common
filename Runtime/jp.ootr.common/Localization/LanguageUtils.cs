using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace jp.ootr.common.Localization
{
    public static class LanguageUtils
    {
        private static readonly Dictionary<string, Language> StrToLang = new Dictionary<string, Language>
        {
            { "en", Language.En },
            { "fr", Language.Fr },
            { "es", Language.Es },
            { "it", Language.It },
            { "ko", Language.Ko },
            { "de", Language.De },
            { "ja", Language.Ja },
            { "pl", Language.Pl },
            { "ru", Language.Ru },
            { "pt_BR", Language.PtBR },
            { "zh_CN", Language.ZhCn },
            { "zh_HK", Language.ZhHk },
            { "he", Language.He },
            { "tok", Language.Tok },
            { "uk", Language.Uk },
        };

        public static Language? FromStr(string langStr)
        {
            return StrToLang.TryGetValue(langStr, out var lang) ? lang : (Language?)null;
        }

        public static Language GetCurrentLanguage()
        {
            var langStr = VRCPlayerApi.GetCurrentLanguage();
            var lang = FromStr(langStr);
            if (lang == null)
            {
                Debug.LogWarning($"Unsupported language: {langStr}, fallback to en");
                return Language.En;
            }
            return lang.Value;
        }

        public static string ToStr(this Language lang)
        {
            switch (lang)
            {
                case Language.En:
                    return "en";
                case Language.Fr:
                    return "fr";
                case Language.Es:
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
