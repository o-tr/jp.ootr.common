using JetBrains.Annotations;
using VRC.SDK3.Data;

namespace jp.ootr.common
{
    public static class DataContainerUtils
    {
        [NotNull]
        public static string[] ToStringArray([NotNull]this DataList list)
        {
            var length = list.Count;
            var array = new string[length];
            for (var i = 0; i < length; i++)
            {
                list.TryGetValue(i, TokenType.String, out var value);
                array[i] = value.String;
            }

            return array;
        }

        public static bool TryToStringArray(this DataToken token, [CanBeNull]out string[] array)
        {
            if (token.TokenType != TokenType.DataList)
            {
                array = null;
                return false;
            }

            return token.DataList.TryToStringArray(out array);
        }

        public static bool TryToStringArray([NotNull]this DataList list, [CanBeNull]out string[] array)
        {
            var length = list.Count;
            array = new string[length];
            for (var i = 0; i < length; i++)
            {
                if (!list.TryGetValue(i, TokenType.String, out var value))
                {
                    array = null;
                    return false;
                }

                array[i] = value.String;
            }

            return true;
        }

        public static bool IsStringArray(this DataToken token)
        {
            if (token.TokenType != TokenType.DataList) return false;
            var list = token.DataList;
            var length = list.Count;
            for (var i = 0; i < length; i++)
                if (!list.TryGetValue(i, TokenType.String, out var _1))
                    return false;

            return true;
        }

        public static bool IsStringDictionary(this DataToken token)
        {
            if (token.TokenType != TokenType.DataDictionary) return false;
            var dict = token.DataDictionary;
            var keys = dict.GetKeys().ToStringArray();
            for (var i = 0; i < dict.Count; i++)
                if (!dict.TryGetValue(keys[i], TokenType.String, out var _1))
                    return false;

            return true;
        }
    }
}
