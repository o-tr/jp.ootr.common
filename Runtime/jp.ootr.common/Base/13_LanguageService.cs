using UnityEngine;
using jp.ootr.common.Localization;

namespace jp.ootr.common.Base
{
    
    public class BaseClass__LanguageService : BaseClass__Sync {
        [SerializeField] protected Localization.Language defaultLanguage = Localization.Language.En;
        private Localization.Language _currentLanguage = Localization.Language.En;

        private void OnEnable()
        {
            _currentLanguage = LanguageUtils.GetCurrentLanguage();
        }

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
