using TMPro;
using UnityEngine;

namespace jp.ootr.common.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationApplierTextMeshPro : MonoBehaviour
    {
        [SerializeField] protected string textKey;
        public string TextKey => textKey;
        public TextMeshProUGUI TextMeshProUGUI => gameObject.GetComponent<TextMeshProUGUI>();
    }
}
