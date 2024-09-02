using UnityEngine;

namespace jp.ootr.common
{
    public static class TransformUtils
    {
        public static Transform[] GetChildren(this Transform transform)
        {
            var children = new Transform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++) children[i] = transform.GetChild(i);

            return children;
        }

        public static void ClearChildren(this Transform transform)
        {
            var list = new GameObject[transform.childCount];
            var count = 0;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.StartsWith("_")) continue;
                list[count++] = child.gameObject;
            }

            for (var i = 0; i < count; i++) Object.DestroyImmediate(list[i]);
        }
    }
}