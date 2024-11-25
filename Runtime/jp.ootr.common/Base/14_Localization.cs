using JetBrains.Annotations;
using jp.ootr.common.Localization;
using TMPro;
using UnityEngine;

namespace jp.ootr.common.Base
{
    public class BaseClass__Localization : BaseClass__LanguageService
    {
        [ItemCanBeNull] [SerializeField] internal string[] localizationKeys;
        [ItemCanBeNull] [SerializeField] internal string[] localizationValues;
        [ItemCanBeNull] [SerializeField] internal string[] localizationTargetKeys;
        [ItemCanBeNull] [SerializeField] internal TextMeshProUGUI[] localizationTargets;

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateLocalization();
        }

        protected override void OnLangChanged(Localization.Language language)
        {
            base.OnLangChanged(language);
            UpdateLocalization();
        }

        protected void UpdateLocalization()
        {
            for (var i = 0; i < localizationTargets.Length; i++)
            {
                if (localizationTargets[i] == null) continue;
                localizationTargets[i].text = GetLocalizedString(localizationTargetKeys[i]);
            }
        }

        protected string __(string key)
        {
            return GetLocalizedString(key);
        }

        [CanBeNull]
        protected string GetLocalizedString([CanBeNull] string key)
        {
            if (localizationKeys.Has($"{CurrentLanguage.ToStr()}.{key}", out var index))
                return localizationValues[index];
            if (localizationKeys.Has($"{defaultLanguage.ToStr()}.{key}", out index)) return localizationValues[index];
            return key;
        }
    }
}
