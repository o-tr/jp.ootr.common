using UnityEngine;

namespace jp.ootr.common.Editor
{
    [CreateAssetMenu(fileName = "ColorPreset", menuName = "ootr/Common/ColorPreset")]
    public class ColorPreset : ScriptableObject
    {
        public Color[] colors = new Color[0];
        public string[] names = new string[0];
    }
}
