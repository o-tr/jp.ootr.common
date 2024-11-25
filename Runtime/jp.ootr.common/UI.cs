using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace jp.ootr.common
{
    public static class UI
    {
        public static bool HasChecked([CanBeNull] [ItemCanBeNull] this Toggle[] toggles)
        {
            return toggles.HasChecked(out var tmp);
        }

        public static bool HasChecked([CanBeNull] [ItemCanBeNull] this Toggle[] toggles, out int index)
        {
            if (toggles == null)
            {
                index = -1;
                return false;
            }

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

        public static bool HasChecked([CanBeNull] [ItemCanBeNull] this GameObject[] buttons, out int index)
        {
            if (buttons == null)
            {
                index = -1;
                return false;
            }

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

        public static bool HasChecked([CanBeNull] [ItemCanBeNull] this Toggle[] identifiers,
            [CanBeNull] [ItemCanBeNull] Slider[] indexes, out int index)
        {
            if (identifiers == null || indexes == null)
            {
                index = -1;
                return false;
            }

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

        public static void ToListChildrenHorizontal([CanBeNull] this Transform obj, int gap = 0, int padding = 0,
            bool adjustWidth = false, bool reverse = false)
        {
            if (obj == null) return;
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            var height = rectTransform.rect.height - padding * 2;
            float x = padding;
            for (var i = 0; i < obj.childCount; i++)
            {
                var item = obj.GetChild(reverse ? obj.childCount - i - 1 : i);
                if (item == null || !item.gameObject.activeSelf) continue;
                var rect = item.gameObject.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(x, -padding);
                var anchorHeight1 = rect.rect.height * (rect.anchorMax.y - rect.anchorMin.y);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, height - anchorHeight1);
                x += rect.rect.width + gap;
            }

            if (!adjustWidth) return;
            var anchorHeight = rectTransform.rect.height * (rectTransform.anchorMax.y - rectTransform.anchorMin.y);
            var anchorWidth = rectTransform.rect.width * (rectTransform.anchorMax.x - rectTransform.anchorMin.x);
            rectTransform.sizeDelta =
                new Vector2(x - gap + padding - anchorWidth, rectTransform.rect.height - anchorHeight);
        }

        public static void ToListChildrenVertical([CanBeNull] this Transform obj, int gap = 0, int padding = 0,
            bool adjustHeight = false, bool reverse = false)
        {
            if (obj == null) return;
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            var width = rectTransform.rect.width - padding * 2;
            float y = -padding;
            for (var i = 0; i < obj.childCount; i++)
            {
                var item = obj.GetChild(reverse ? obj.childCount - i - 1 : i);
                if (item == null || !item.gameObject.activeSelf) continue;
                var rect = item.gameObject.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(padding, y);
                var anchorWidth1 = rect.rect.width * (rect.anchorMax.x - rect.anchorMin.x);
                rect.sizeDelta = new Vector2(width - anchorWidth1, rect.sizeDelta.y);
                y -= rect.rect.height + gap;
            }

            if (!adjustHeight) return;
            var anchorWidth = rectTransform.rect.width * (rectTransform.anchorMax.x - rectTransform.anchorMin.x);
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width - anchorWidth, -y - gap + padding);
        }

        public static void ToFillChildrenHorizontal([CanBeNull] this Transform obj, int gap = 0, int padding = 0)
        {
            if (obj == null) return;
            var activeItemCount = 0;
            var flexibleCount = 0;
            var width = new float[obj.childCount];
            float fixedWidth = 0;
            foreach (Transform item in obj)
            {
                if (item == null || !item.gameObject.activeSelf) continue;
                var layoutElement = item.gameObject.GetComponent<LayoutElement>();
                if (layoutElement != null && layoutElement.preferredWidth > 0)
                {
                    fixedWidth += layoutElement.preferredWidth;
                    width[activeItemCount++] = layoutElement.preferredWidth;
                    continue;
                }

                activeItemCount++;
                flexibleCount++;
            }

            var gapSpace = gap * (activeItemCount - 1);
            var itemWidth = (obj.GetComponent<RectTransform>().rect.width - fixedWidth - gapSpace - padding * 2) /
                            flexibleCount;
            float x = padding;
            var index = 0;
            foreach (Transform item in obj)
            {
                if (!item.gameObject.activeSelf) continue;
                var rectTransform = item.gameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(x, 0);
                var fixedWidthValue = width[index++];
                if (fixedWidthValue > 0)
                {
                    rectTransform.sizeDelta = new Vector2(fixedWidthValue, rectTransform.sizeDelta.y);
                    x += fixedWidthValue + gap;
                    continue;
                }

                rectTransform.sizeDelta = new Vector2(itemWidth, rectTransform.sizeDelta.y);
                x += itemWidth + gap;
            }
        }

        public static void ToFillChildrenVertical([CanBeNull] this Transform obj, int gap = 0, int padding = 0)
        {
            if (obj == null) return;
            var activeItemCount = 0;
            var flexibleCount = 0;
            var height = new float[obj.childCount];
            float fixedHeight = 0;
            foreach (Transform item in obj)
            {
                if (!item.gameObject.activeSelf) continue;
                var layoutElement = item.gameObject.GetComponent<LayoutElement>();
                if (layoutElement != null && layoutElement.preferredHeight > 0)
                {
                    fixedHeight += layoutElement.preferredHeight;
                    height[activeItemCount++] = layoutElement.preferredHeight;
                    continue;
                }

                activeItemCount++;
                flexibleCount++;
            }

            var gapSpace = gap * (activeItemCount - 1);
            var itemHeight = (obj.GetComponent<RectTransform>().rect.height - fixedHeight - gapSpace - padding * 2) /
                             flexibleCount;
            float y = -padding;
            var index = 0;
            foreach (Transform item in obj)
            {
                if (!item.gameObject.activeSelf) continue;
                var rectTransform = item.gameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, y);
                var fixedHeightValue = height[index++];
                if (fixedHeightValue > 0)
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, fixedHeightValue);
                    y -= fixedHeightValue + gap;
                    continue;
                }

                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, itemHeight);
                y -= itemHeight + gap;
            }
        }

        public static void CreateButton(int index, [CanBeNull] string value, [CanBeNull] GameObject original,
            [CanBeNull] out GameObject button,
            [CanBeNull] out Animator animator, [CanBeNull] out InputField input, [CanBeNull] out Slider slider,
            [CanBeNull] out Toggle toggle)
        {
            if (original == null)
            {
                button = null;
                animator = null;
                input = null;
                slider = null;
                toggle = null;
                return;
            }

            button = Object.Instantiate(original, original.transform.parent);
            button.SetActive(true);
            animator = button.GetComponent<Animator>();
            input = button.transform.Find("__VALUE").GetComponent<InputField>();
            slider = button.transform.Find("__INDEX").GetComponent<Slider>();
            toggle = button.transform.Find("__IDENTIFIER").GetComponent<Toggle>();
            if (animator == null || input == null || slider == null || toggle == null)
            {
                Object.Destroy(button);
                button = null;
                animator = null;
                input = null;
                slider = null;
                toggle = null;
                return;
            }

            slider.value = index;
            input.text = value;
            toggle.isOn = false;
        }

        public static void Update([CanBeNull] this VerticalLayoutGroup layoutGroup)
        {
            if (layoutGroup == null) return;
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.SetLayoutVertical();
            layoutGroup.SetLayoutHorizontal();
        }

        public static void Update([CanBeNull] this HorizontalLayoutGroup layoutGroup)
        {
            if (layoutGroup == null) return;
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.SetLayoutVertical();
            layoutGroup.SetLayoutHorizontal();
        }
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }
}
