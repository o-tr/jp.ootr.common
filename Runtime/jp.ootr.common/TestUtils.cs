#if UNITY_EDITOR
using UnityEngine.Assertions;

namespace jp.ootr.common
{
    public static class TestUtils
    {
        public static void AreEqual<T>(this T[] array, T[] target)
        {
            Assert.IsNotNull(array);
            Assert.IsNotNull(target);
            Assert.AreEqual(array.Length, target.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(array[i],target[i]);
            }
        }
    }
}
#endif
