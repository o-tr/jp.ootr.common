#if UNITY_EDITOR
using NUnit.Framework;

namespace jp.ootr.common.Tests.ArrayUtils
{
    public class RemoveTests
    {
        [Test]
        public void Remove()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Remove(0);
            array.AreEqual(new[] { 2, 3, 4, 5 });
        }

        [Test]
        public void RemoveWithOut()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Remove(0, out var item);
            Assert.AreEqual(1, item);
            array.AreEqual(new[] { 2, 3, 4, 5 });
        }

        [Test]
        public void RemoveWithNegativeIndex()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Remove(-1);
            array.AreEqual(new[] { 1, 2, 3, 4 });
        }
    }

    public class AppendTests
    {
        [Test]
        public void Append()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Append(6);
            array.AreEqual(new[] { 1, 2, 3, 4, 5, 6 });
        }
    }

    public class ReplaceTests
    {
        [Test]
        public void Replace()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Replace(new[] { 6, 7, 8 }, 2);
            array.AreEqual(new[] { 1, 2, 6, 7, 8, 4, 5 });
        }

        [Test]
        public void ReplaceWithNegativeIndex()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Replace(new[] { 6, 7, 8 }, -2);
            array.AreEqual(new[] { 1, 2, 3, 6, 7, 8, 5 });
        }
    }

    public class ResizeTests
    {
        [Test]
        public void Resize()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Resize(3);
            array.AreEqual(new[] { 1, 2, 3 });
        }

        [Test]
        public void ResizeWithNegativeLength()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Resize(-2);
            array.AreEqual(new[] { 1, 2, 3 });
        }

        [Test]
        public void ResizeWithZeroLength()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Resize(0);
            array.AreEqual(new int[0]);
        }

        [Test]
        public void ResizeWithNegativeLengthAndZeroLengthArray()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Resize(-10);
            array.AreEqual(new int[0]);
        }
    }

    public class InsertTests
    {
        [Test]
        public void Insert()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(6, 2);
            array.AreEqual(new[] { 1, 2, 6, 3, 4, 5 });
        }

        [Test]
        public void InsertAtLastOfArray()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(2, 5);
            array.AreEqual(new[] { 1, 2, 3, 4, 5, 2 });
        }

        [Test]
        public void InsertOutOfRange()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(6, 10);
            array.AreEqual(new[] { 1, 2, 3, 4, 5 });
        }

        [Test]
        public void InsertWithNegativeIndex()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(6, -2);
            array.AreEqual(new[] { 1, 2, 3, 6, 4, 5 });
        }

        [Test]
        public void InsertArray()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(new[] { 6, 7, 8 }, 2);
            array.AreEqual(new[] { 1, 2, 6, 7, 8, 3, 4, 5 });
        }

        [Test]
        public void InsertArrayWithNegativeIndex()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(new[] { 6, 7, 8 }, -2);
            array.AreEqual(new[] { 1, 2, 3, 6, 7, 8, 4, 5 });
        }

        [Test]
        public void InsertArrayOutOfRange()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(new[] { 6, 7, 8 }, 10);
            array.AreEqual(new[] { 1, 2, 3, 4, 5 });
        }

        [Test]
        public void InsertArrayAtLastOfArray()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(new[] { 6, 7, 8 }, 5);
            array.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [Test]
        public void InsertArrayWithNegativeIndexAndOutOfRange()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Insert(new[] { 6, 7, 8 }, -10);
            array.AreEqual(new[] { 1, 2, 3, 4, 5 });
        }
    }

    public class HasTests
    {
        [Test]
        public void Has()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            Assert.IsTrue(array.Has(1));
            Assert.IsTrue(array.Has(5));
            Assert.IsFalse(array.Has(0));
            Assert.IsFalse(array.Has(6));
        }

        [Test]
        public void HasWithOut()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            Assert.IsTrue(array.Has(1, out var index));
            Assert.AreEqual(0, index);
            Assert.IsTrue(array.Has(5, out index));
            Assert.AreEqual(4, index);
            Assert.IsFalse(array.Has(0, out index));
            Assert.AreEqual(-1, index);
            Assert.IsFalse(array.Has(6, out index));
            Assert.AreEqual(-1, index);
        }
    }

    public class MergeTests
    {
        [Test]
        public void Merge()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Merge(new[] { 6, 7, 8 });
            array.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [Test]
        public void MergeUnique()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Merge(new[] { 6, 7, 8, 1, 2, 3 }, true);
            array.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }
    }

    public class UniqueTests
    {
        [Test]
        public void Unique()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Unique();
            array.AreEqual(new[] { 1, 2, 3, 4, 5 });
        }

        [Test]
        public void UniqueWithZero()
        {
            var array = new[] { 1, 2, 3, 4, 5, 0 };
            array = array.Unique();
            array.AreEqual(new[] { 1, 2, 3, 4, 5, 0 });
        }

        [Test]
        public void UniqueWithZeroAndDuplicate()
        {
            var array = new[] { 1, 2, 3, 4, 5, 0, 0, 0 };
            array = array.Unique();
            array.AreEqual(new[] { 1, 2, 3, 4, 5, 0 });
        }

        [Test]
        public void UniqueWithZeroAndDuplicateAndNegative()
        {
            var array = new[] { 1, 2, 3, 4, 5, 0, 0, 0, -1, -2, -3 };
            array = array.Unique();
            array.AreEqual(new[] { 1, 2, 3, 4, 5, 0, -1, -2, -3 });
        }
    }

    public class ShiftTests
    {
        [Test]
        public void Shift()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Shift();
            array.AreEqual(new[] { 2, 3, 4, 5 });
        }

        [Test]
        public void ShiftWithOut()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Shift(out var item);
            Assert.AreEqual(1, item);
            array.AreEqual(new[] { 2, 3, 4, 5 });
        }

        [Test]
        public void ShiftWithZeroLength()
        {
            var array = new int[0];
            array = array.Shift(out var _void, out var success);
            Assert.IsFalse(success);
            array.AreEqual(new int[0]);
        }
    }

    public class PopTests
    {
        [Test]
        public void Pop()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Pop();
            array.AreEqual(new[] { 1, 2, 3, 4 });
        }

        [Test]
        public void PopWithOut()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array = array.Pop(out var item);
            Assert.AreEqual(5, item);
            array.AreEqual(new[] { 1, 2, 3, 4 });
        }

        [Test]
        public void PopWithZeroLength()
        {
            var array = new int[0];
            array = array.Pop(out var _void, out var success);
            Assert.IsFalse(success);
            array.AreEqual(new int[0]);
        }
    }

    public class DiffTests
    {
        [Test]
        public void Diff()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array.Diff(new[] { 1, 2, 3, 4, 5 }, out var removed, out var added);
            removed.AreEqual(new int[0]);
            added.AreEqual(new int[0]);
        }

        [Test]
        public void DiffWithRemoved()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            array.Diff(new[] { 1, 2, 3, 4 }, out var removed, out var added);
            removed.AreEqual(new[] { 4 });
            added.AreEqual(new int[0]);
        }

        [Test]
        public void DiffWithAdded()
        {
            var array = new[] { 1, 2, 3, 4 };
            array.Diff(new[] { 1, 2, 3, 4, 5 }, out var removed, out var added);
            removed.AreEqual(new int[0]);
            added.AreEqual(new[] { 4 });
        }
    }
}
#endif
