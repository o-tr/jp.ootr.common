using VRC.SDK3.Data;

namespace jp.ootr.common
{
    public static class DataContainerUtils
    {
        public static string[] ToStringArray(this DataList list)
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
    }
}