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
        public TextMeshProUGUI TextMeshProUGUI { get; private set; }

        private void Awake()
        {
            TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }
    }
}
#endif
