using UnityEngine;

namespace jp.ootr.common
{
    public static class EditorStyle
    {
        public static GUIStyle UiTitle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 24,
            normal = { textColor = Color.white }
        };
    }
}