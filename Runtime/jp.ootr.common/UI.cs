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

        public static void ToListChildren(this Transform obj,int gap = 0, int padding = 0, bool adjustParent = false, bool reverse = false, Direction direction = Direction.Vertical)
        {
            if (direction == Direction.Horizontal)
                obj.UIListItemsHorizontal(gap, padding, adjustParent, reverse);
            else
                obj.UIListItemsVertical(gap, padding, adjustParent, reverse);
        }
        
        private static void UIListItemsHorizontal(this Transform obj, int gap = 0, int padding = 0, bool adjustWidth = false, bool reverse = false)
        {
            float height = obj.GetComponent<RectTransform>().rect.height - padding * 2;
            float x = padding;
            for (var i = 0; i < obj.childCount; i++)
            {
                var item = obj.GetChild(reverse ? obj.childCount - i - 1 : i);
                if (!item.gameObject.activeSelf) continue;
                var rect = item.gameObject.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(x, padding);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
                x += rect.rect.width + gap;
            }

            if (!adjustWidth) return;
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(x - gap + padding, rectTransform.sizeDelta.y);
        }

        private static void UIListItemsVertical(this Transform obj,int gap = 0, int padding = 0, bool adjustHeight = false, bool reverse = false)
        {
            float width = obj.GetComponent<RectTransform>().rect.width - padding * 2;
            float y = -padding;
            for (var i = 0; i < obj.childCount; i++)
            {
                var item = obj.GetChild(reverse ? obj.childCount - i - 1 : i);
                if (!item.gameObject.activeSelf) continue;
                var rect = item.gameObject.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(padding, y);
                rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
                y -= rect.rect.height + gap;
            }

            if (!adjustHeight) return;
            var rectTransform = obj.gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, -y - gap + padding);
        }

        public static void ToFillChildren(this Transform obj, Direction direction, int gap = 0, int padding = 0)
        {
            if (direction == Direction.Horizontal)
                UIFillItemsHorizontal(obj, gap, padding);
            else
                UIFillItemsVertical(obj, gap, padding);
        }

        private static void UIFillItemsHorizontal(Transform obj, int gap = 0, int padding = 0)
        {
            var activeItemCount = 0;
            var flexibleCount = 0;
            var width = new float[obj.childCount];
            float fixedWidth = 0;
            foreach (Transform item in obj)
            {
                if (!item.gameObject.activeSelf) continue;
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
            var itemWidth = (obj.GetComponent<RectTransform>().rect.width - fixedWidth - gapSpace - padding * 2) / flexibleCount;
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

        private static void UIFillItemsVertical(Transform obj, int gap = 0, int padding = 0)
        {
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
            var itemHeight = (obj.GetComponent<RectTransform>().rect.height - fixedHeight - gapSpace - padding * 2) / flexibleCount;
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
                    rectTransform.sizeDelta = new Vector2(fixedHeightValue, rectTransform.sizeDelta.y);
                    y -= fixedHeightValue + gap;
                    continue;
                }
                rectTransform.sizeDelta = new Vector2(itemHeight, rectTransform.sizeDelta.y);
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
        
        public static void Update(this VerticalLayoutGroup layoutGroup)
        {
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.SetLayoutVertical();
            layoutGroup.SetLayoutHorizontal();
        }
        public static void Update(this HorizontalLayoutGroup layoutGroup)
        {
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