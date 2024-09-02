using UnityEngine;

namespace jp.ootr.common
{
    public static class String
    {
        public static string[] Split(this string str, int sliceCount)
        {
            if (sliceCount <= 0)
            {
                Console.Warn($"SplitString: Slice count is less than 0: {sliceCount}");
                return new string[0];
            }

            if (str.Length <= sliceCount) return new[] { str };
            var len = (int)Mathf.Ceil(str.Length / (float)sliceCount);
            var result = new string[len];
            for (var i = 0; i < len; i++)
                result[i] = str.Substring(i * sliceCount, Mathf.Min(sliceCount, str.Length - i * sliceCount));
            return result;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
