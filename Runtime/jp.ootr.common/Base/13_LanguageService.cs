using System;
using UnityEngine;
using VRC.SDKBase;

namespace jp.ootr.common.Base
{
    public enum Language
    {
        en,
        ja,
    }
    
    public class BaseClass__LanguageService : BaseClass__Sync {
        [SerializeField] protected Language defaultLanguage = Language.en;
        private Language _currentLanguage = Language.en;

        private void OnEnable()
        {
            _currentLanguage = LanguageUtils.GetCurrentLanguage();
        }

        protected Language CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage == value) return;
                _currentLanguage = value;
                OnLangChanged(value);
            }
        }

        public override void OnLanguageChanged(string language)
        {
            base.OnLanguageChanged(language);
            var lang = LanguageUtils.GetCurrentLanguage();
            if (_currentLanguage == lang) return;
            _currentLanguage = lang;
            OnLangChanged(lang);
        }

        protected virtual void OnLangChanged(Language lang)
        {
        }
    }

    public static class LanguageUtils
    {
        public static Language GetCurrentLanguage()
        {
            var langStr = VRCPlayerApi.GetCurrentLanguage();
            switch (langStr)
            {
                case "ja":
                    return Language.ja;
                case "en":
                    return Language.en;
                default:
                    Debug.LogWarning($"Unsupported language: {langStr}, fallback to en");
                    return Language.en;
            }
        }
        
        public static string ToStr(this Language lang)
        {
            switch (lang)
            {
                case Language.ja:
                    return "ja";
                case Language.en:
                    return "en";
                default:
                    return "en";
            }
        }
    }
}