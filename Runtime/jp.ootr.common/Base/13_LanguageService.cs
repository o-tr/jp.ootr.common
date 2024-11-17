using jp.ootr.common.Localization;
using UnityEngine;

namespace jp.ootr.common.Base
{
    public class BaseClass__LanguageService : BaseClass__Sync
    {
        [SerializeField] internal Localization.Language defaultLanguage = Localization.Language.En;
        [SerializeField] internal Localization.Language[] supportedLanguages = new Localization.Language[0];
        private Localization.Language _currentLanguage = Localization.Language.En;

        protected Localization.Language CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage == value) return;
                _currentLanguage = value;
                OnLangChanged(value);
            }
        }

        protected virtual void OnEnable()
        {
            _currentLanguage = LanguageUtils.GetCurrentLanguage();
        }

        public override void OnLanguageChanged(string language)
        {
            base.OnLanguageChanged(language);
            var lang = LanguageUtils.GetCurrentLanguage();
            if (_currentLanguage == lang) return;
            _currentLanguage = lang;
            OnLangChanged(lang);
        }

        protected virtual void OnLangChanged(Localization.Language lang)
        {
        }
    }
}
