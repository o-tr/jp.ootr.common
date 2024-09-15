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
    

    public class AppendTests
    {
        [Test]
        public void Append()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Append(6);
            array.AreEqual(new[] {1, 2, 3, 4, 5, 6});
        }
    }

    public class ReplaceTests
    {
        
        [Test]
        public void Replace()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Replace(new[] {6, 7, 8}, 2);
            array.AreEqual(new[] {1, 2, 6, 7, 8, 4, 5});
        }
        
        [Test]
        public void ReplaceWithNegativeIndex()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Replace(new[] {6, 7, 8}, -2);
            array.AreEqual(new[] {1, 2, 3, 6, 7, 8, 5});
        }
    }
    
    public class ResizeTests
    {
        [Test]
        public void Resize()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Resize(3);
            array.AreEqual(new[] {1, 2, 3});
        }
        
        [Test]
        public void ResizeWithNegativeLength()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Resize(-2);
            array.AreEqual(new[] {1, 2, 3});
        }
        
        [Test]
        public void ResizeWithZeroLength()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Resize(0);
            array.AreEqual(new int[0]);
        }
        
        [Test]
        public void ResizeWithNegativeLengthAndZeroLengthArray()
        {
            var array = new[] {1, 2, 3, 4, 5};
            array = array.Resize(-10);
            array.AreEqual(new int[0]);
        }
    }
}