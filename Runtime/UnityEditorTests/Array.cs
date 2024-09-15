using NUnit.Framework;

namespace jp.ootr.common.Tests.ArrayUtils
{
    public class RemoveTests
    {
        [Test]
        public void Remove()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Remove(0);
            array.AreEqual(new[] {2, 3, 4, 5});
        }

        [Test]
        public void RemoveWithOut()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Remove(0, out var item);
            Assert.AreEqual(1, item);
            array.AreEqual(new[] {2, 3, 4, 5});
        }
        
        [Test]
        public void RemoveWithNegativeIndex()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Remove(-1);
            array.AreEqual(new[] {1, 2, 3, 4});
        }
    }
    
}