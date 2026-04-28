#if UNITY_EDITOR
using TMPro;
using UnityEngine;

namespace jp.ootr.common.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationApplierTextMeshPro : EditorOnlyMonoBehaviour
    {
        [SerializeField] protected string textKey;
        public string TextKey => textKey;
        private TextMeshProUGUI _textMeshProUGUI;

        public TextMeshProUGUI TextMeshProUGUI => _textMeshProUGUI != null
            ? _textMeshProUGUI
            : (_textMeshProUGUI = GetComponent<TextMeshProUGUI>());
    }
}
#endif
