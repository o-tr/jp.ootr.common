using NUnit.Framework;

namespace jp.ootr.common.Tests.ByteUtils
{
    public class SimilarTests
    {
        [Test]
        public void Similar()
        {
            var data1 = new byte[] {1, 2, 3, 4, 5};
            var data2 = new byte[] {1, 2, 3, 4, 5};
            Assert.IsTrue(data1.Similar(data2));
        }
        
        [Test]
        public void SimilarWithSampleRate()
        {
            var data1 = new byte[] {1, 2, 3, 4, 5};
            var data2 = new byte[] {1, 2, 3, 4, 5};
            Assert.IsTrue(data1.Similar(data2, 1.0f));
        }
        
        [Test]
        public void SimilarWithSampleSize()
        {
            var data1 = new byte[] {1, 2, 3, 4, 5};
            var data2 = new byte[] {1, 2, 3, 4, 5};
            Assert.IsTrue(data1.Similar(data2, 5));
        }
        
        [Test]
        public void SimilarWithDifferentData()
        {
            var data1 = new byte[] {1, 2, 3, 4, 5};
            var data2 = new byte[] {0,0,0,0,0};
            Assert.IsFalse(data1.Similar(data2));
        }
        
        [Test]
        public void SimilarWithDifferentLength()
        {
            var data1 = new byte[] {1, 2, 3, 4, 5};
            var data2 = new byte[] {1, 2, 3, 4, 5, 6};
            Assert.IsFalse(data1.Similar(data2));
        }
        
        [Test]
        public void SimilarWithNullData1()
        {
            byte[] data1 = null;
            var data2 = new byte[] {1, 2, 3, 4, 5};
            Assert.IsFalse(data1.Similar(data2));
        }
    }

    public class MayBlankTests
    {
        [Test]
        public void MayBlank()
        {
            var data = new byte[] {1, 2, 3, 4, 5,6,7,8};
            Assert.IsFalse(data.MayBlank());
        }
        
        [Test]
        public void MayBlankWithSampleRate()
        {
            var data = new byte[] {1, 2, 3, 4,  5,6,7,8};
            Assert.IsFalse(data.MayBlank(1.0f));
        }
        
        [Test]
        public void MayBlankWithSampleSize()
        {
            var data = new byte[] {1, 2, 3, 4, 5,6,7,8};
            Assert.IsFalse(data.MayBlank(5));
        }
        
        [Test]
        public void MayBlankWithBlankData()
        {
            var data = new byte[] {0, 0, 0, 0, 0, 0, 0, 0};
            Assert.IsTrue(data.MayBlank());
        }
    }
}
