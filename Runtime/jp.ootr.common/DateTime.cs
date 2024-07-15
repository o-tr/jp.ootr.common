using System;

namespace jp.ootr.common
{
    public static class TimeUtil
    {
        public static ulong ToUnixTime(this DateTime dt)
        {
            return (ulong)(dt - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}