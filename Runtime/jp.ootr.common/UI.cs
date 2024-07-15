using UnityEngine;
using UnityEngine.UI;

namespace jp.ootr.common
{
    public static class UI
    {
        public static bool HasChecked(this Toggle[] toggles)
        {
            return toggles.HasChecked(out var tmp);
        }

        public static bool HasChecked(this Toggle[] toggles, out int index)
        {
            for (var i = 0; i < toggles.Length; i++)
            {
                if (toggles[i] == null) continue;
                if (!toggles[i].isOn) continue;
                toggles[i].isOn = false;
                index = i;
                return true;
            }

            index = -1;
            return false;
        }

        public static bool HasChecked(this GameObject[] buttons, out int index)
        {
            foreach (var button in buttons)
            {
                if (button == null) continue;
                var toggle = button.transform.Find("__IDENTIFIER").GetComponent<Toggle>();
                if (!toggle.isOn) continue;
                toggle.isOn = false;
                index = (int)button.transform.Find("__INDEX").GetComponent<Slider>().value;
                return true;
            }

            index = -1;
            return false;
        }

        public static bool HasChecked(this Toggle[] identifiers, Slider[] indexes, out int index)
        {
            for (var i = 0; i < identifiers.Length; i++)
            {
                var identifier = identifiers[i];
                if (identifier == null) continue;
                if (!identifier.isOn) continue;
                identifier.isOn = false;
                if (indexes[i] == null) continue;
                index = (int)indexes[i].value;
                return true;
            }

            index = -1;
            return false;
        }

        public static void ToListChildren(this Transform obj, bool adjustHeight = false, bool reverse = false)
        {
            float y = 0;
            for (var i = 0; i < obj.childCount; i++)
            {
                var item = obj.GetChild(reverse ? obj.childCount - i - 1 : i);
                if (!item.gameObject.activeSelf) continue;
                var rect = item.gameObject.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, y);
                y -= rect.rect.height;
            }

            if (!adjustHeight) return;
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, -y);
        }

        public static void ToFillChildren(this Transform obj, FillDirection direction, int gap = 0, int padding = 0)
        {
            if (direction == FillDirection.Horizontal)
                UIFillItemsHorizontal(obj, gap, padding);
            else
                UIFillItemsVertical(obj, gap, padding);
        }

        private static void UIFillItemsHorizontal(Transform obj, int gap = 0, int padding = 0)
        {
            var activeItemCount = 0;
            foreach (Transform item in obj)
            {
                if (!item.gameObject.activeSelf) continue;
                activeItemCount++;
            }

            var gapSpace = gap * (activeItemCount - 1);
            var itemWidth = (obj.GetComponent<RectTransform>().rect.width - gapSpace - padding * 2) / activeItemCount;
            float x = padding;
            foreach (Transform item in obj)
            {
                if (!item.gameObject.activeSelf) continue;
                var rectTransform = item.gameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(x, 0);
                rectTransform.sizeDelta = new Vector2(itemWidth, rectTransform.sizeDelta.y);
                x += itemWidth + gap;
            }
        }

        private static void UIFillItemsVertical(Transform obj, int gap = 0, int padding = 0)
        {
            var activeItemCount = 0;
            foreach (Transform item in obj)
            {
                if (!item.gameObject.activeSelf) continue;
                activeItemCount++;
            }

            var gapSpace = gap * (activeItemCount - 1);
            var itemHeight = (obj.GetComponent<RectTransform>().rect.height - gapSpace - padding * 2) / activeItemCount;
            float y = -padding;
            foreach (Transform item in obj)
            {
                if (!item.gameObject.activeSelf) continue;
                var rectTransform = item.gameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, y);
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, itemHeight);
                y -= itemHeight + gap;
            }
        }

        public static void CreateButton(int index, string value, GameObject original, out GameObject button,
            out Animator animator, out InputField input, out Slider slider, out Toggle toggle)
        {
            button = Object.Instantiate(original, original.transform.parent);
            button.SetActive(true);
            animator = button.GetComponent<Animator>();
            input = button.transform.Find("__VALUE").GetComponent<InputField>();
            slider = button.transform.Find("__INDEX").GetComponent<Slider>();
            toggle = button.transform.Find("__IDENTIFIER").GetComponent<Toggle>();
            slider.value = index;
            input.text = value;
            toggle.isOn = false;
        }
    }

    public enum FillDirection
    {
        Horizontal,
        Vertical
    }
}