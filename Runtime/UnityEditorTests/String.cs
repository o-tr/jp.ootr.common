using NUnit.Framework;

namespace jp.ootr.common.Tests.StringUtils
{
    public class SplitTests
    {
        [Test]
        public void Split()
        {
            var str = "a,b,c";
            var result = str.Split(',');
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
            Assert.AreEqual("c", result[2]);
        }
        
        [Test]
        public void SplitWithEmptyString()
        {
            var str = "";
            var result = str.Split(',');
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("", result[0]);
        }
    }
    
    public static class IsNullOrEmptyTests
    {
        [Test]
        public static void IsNullOrEmpty()
        {
            Assert.IsTrue(String.IsNullOrEmpty(null));
            Assert.IsTrue("".IsNullOrEmpty());
            Assert.IsFalse(" ".IsNullOrEmpty());
            Assert.IsFalse("a".IsNullOrEmpty());
        }
    }
}
