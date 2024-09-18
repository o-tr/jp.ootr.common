using NUnit.Framework;

namespace jp.ootr.common.Tests.DateTimeUtils
{
    public static class ToUnixTimeTests
    {
        [Test]
        public static void ToUnixTime()
        {
            var dt = new System.DateTime(2021, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            Assert.AreEqual(1609459200, dt.ToUnixTime());
        }
    }
}
