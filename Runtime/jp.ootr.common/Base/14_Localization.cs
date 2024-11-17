using jp.ootr.common.Localization;
using TMPro;
using UnityEngine;

namespace jp.ootr.common.Base
{
    
    public class BaseClass__Localization : BaseClass__LanguageService {
        [SerializeField] internal string[] localizationKeys;
        [SerializeField] internal string[] localizationValues;
        [SerializeField] internal string[] localizationTargetKeys;
        [SerializeField] internal TextMeshProUGUI[] localizationTargets;

        protected override void OnLangChanged(Localization.Language language)
        {
            base.OnLangChanged(language);
            UpdateLocalization();
        }

        protected void UpdateLocalization()
        {
            for (var i = 0; i < localizationTargets.Length; i++)
            {
                localizationTargets[i].text = GetLocalizedString(localizationTargetKeys[i]);
            }
        }
        
        protected string __(string key)
        {
            return GetLocalizedString(key);
        }
        
        protected string GetLocalizedString(string key)
        {
            if (localizationKeys.Has($"{CurrentLanguage.ToStr()}.{key}", out var index))
            {
                return localizationValues[index];
            }
            if (localizationKeys.Has($"{defaultLanguage.ToStr()}.{key}", out index))
            {
                return localizationValues[index];
            }
            return key;
        }
    }
}
