using System;
using JetBrains.Annotations;
using UnityEngine;

namespace jp.ootr.common
{
    public static class ArrayUtils
    {
        public const string PackageName = "jp.ootr.common.ArrayUtils";


        [NotNull]
        public static T[] Remove<T>([NotNull] this T[] array, int index)
        {
            return array.Remove(index, out var void1);
        }

        [NotNull]
        public static T[] Remove<T>([NotNull] this T[] array, int index, [CanBeNull] out T item)
        {
            if (index < 0) index = array.Length + index;

            if (index < 0 || index >= array.Length)
            {
                Console.Warn($"RemoveItemFromArray: Index out of range: {index}, array length: {array.Length}",
                    PackageName);
                item = default;
                return array;
            }

            item = array[index];
            var tmpArray = new T[array.Length - 1];
            Array.Copy(array, 0, tmpArray, 0, index);
            Array.Copy(array, index + 1, tmpArray, index, array.Length - index - 1);
            return tmpArray;
        }

        [NotNull]
        public static T[] Append<T>([NotNull] this T[] array, [CanBeNull] T item)
        {
            var tmpArray = new T[array.Length + 1];
            array.CopyTo(tmpArray, 0);
            tmpArray[array.Length] = item;
            return tmpArray;
        }

        [NotNull]
        public static T[] Replace<T>([NotNull] this T[] array, [NotNull] T[] items, int index)
        {
            if (index < 0) index = array.Length + index;

            if (index < 0 || index >= array.Length)
            {
                Console.Warn($"ReplaceItemWithArray: Index out of range: {index}, array length: {array.Length}",
                    PackageName);
                return array;
            }

            var tmpArray = new T[array.Length + items.Length - 1];
            Array.Copy(array, 0, tmpArray, 0, index);
            Array.Copy(items, 0, tmpArray, index, items.Length);
            Array.Copy(array, index + 1, tmpArray, index + items.Length, array.Length - index - 1);
            return tmpArray;
        }

        [NotNull]
        public static T[] Resize<T>([NotNull] this T[] array, int targetLength)
        {
            if (array.Length == targetLength) return array;

            if (targetLength < 0) targetLength = array.Length + targetLength;

            if (targetLength < 0)
            {
                Console.Warn($"ResizeArray: Target length is less than 0: {targetLength}", PackageName);
                return new T[0];
            }

            var tmpArray = new T[targetLength];
            if (array.Length == 0) return tmpArray;
            Array.Copy(array, 0, tmpArray, 0, Mathf.Min(array.Length, targetLength));
            return tmpArray;
        }

        [NotNull]
        public static T[] Insert<T>([NotNull] this T[] array, [CanBeNull] T item, int index)
        {
            if (index < 0) index = array.Length + index;

            if (index < 0 || index > array.Length)
            {
                Console.Warn($"InsertItemToArray: Index out of range: {index}, array length: {array.Length}",
                    PackageName);
                return array;
            }

            var tmpArray = new T[array.Length + 1];
            Array.Copy(array, 0, tmpArray, 0, index);
            tmpArray[index] = item;
            Array.Copy(array, index, tmpArray, index + 1, array.Length - index);
            return tmpArray;
        }

        [NotNull]
        public static T[] Insert<T>([NotNull] this T[] array, [NotNull] T[] items, int index)
        {
            return array.Insert(items, index, items.Length);
        }

        [NotNull]
        public static T[] Insert<T>([NotNull] this T[] array, [NotNull] T[] items, int index, int itemsLength)
        {
            if (index < 0) index = array.Length + index;

            if (index < 0 || index > array.Length)
            {
                Console.Warn($"InsertArrayToArray: Index out of range: {index}, array length: {array.Length}",
                    PackageName);
                return array;
            }

            var tmpArray = new T[array.Length + itemsLength];
            Array.Copy(array, 0, tmpArray, 0, index);
            Array.Copy(items, 0, tmpArray, index, itemsLength);
            Array.Copy(array, index, tmpArray, index + itemsLength, array.Length - index);
            return tmpArray;
        }

        public static bool Has<T>([NotNull] this T[] array, [CanBeNull] T item)
        {
            return array.Has(item, out var void1);
        }

        public static bool Has<T>([NotNull] this T[] array, [CanBeNull] T item, out int index)
        {
            index = Array.IndexOf(array, item);
            return index != -1;
        }

        [NotNull]
        public static T[] Merge<T>([NotNull] this T[] array, [NotNull] T[] items, bool unique = false)
        {
            var tmpArray = new T[array.Length + items.Length];
            Array.Copy(array, 0, tmpArray, 0, array.Length);
            Array.Copy(items, 0, tmpArray, array.Length, items.Length);
            return unique ? tmpArray.Unique() : tmpArray;
        }

        [NotNull]
        public static T[] Unique<T>([NotNull] this T[] array)
        {
            if (typeof(T) == typeof(int))
            {
                var result = new T[array.Length];

                var isZeroContained = false;
                var tmpIndex = 0;

                foreach (var item in array)
                {
                    if (item.Equals(0) && !isZeroContained)
                    {
                        isZeroContained = true;
                        result[tmpIndex++] = (T)(object)0;
                        continue;
                    }

                    if (result.Has(item)) continue;
                    result[tmpIndex++] = item;
                }

                return result.Resize(tmpIndex);
            }

            {
                var result = new T[array.Length];
                var tmpIndex = 0;

                foreach (var item in array)
                {
                    if (result.Has(item)) continue;
                    result[tmpIndex++] = item;
                }

                return result.Resize(tmpIndex);
            }
        }

        [Obsolete("Use ArrayUtils.Unique instead")]
        [NotNull]
        public static int[] IntUnique([NotNull] this int[] array)
        {
            return array.Unique();
        }

        [NotNull]
        public static T[] Shift<T>([NotNull] this T[] array)
        {
            return array.Shift(out var void1, out var void2);
        }

        [NotNull]
        public static T[] Shift<T>([NotNull] this T[] array, [CanBeNull] out T item)
        {
            return array.Shift(out item, out var void1);
        }

        [NotNull]
        public static T[] Shift<T>([NotNull] this T[] array, [CanBeNull] out T item, out bool success)
        {
            if (array.Length == 0)
            {
                item = default;
                success = false;
                return array;
            }

            item = array[0];
            var tmpArray = new T[array.Length - 1];
            Array.Copy(array, 1, tmpArray, 0, array.Length - 1);
            success = true;
            return tmpArray;
        }

        [Obsolete("Use ArrayUtils.Shift instead")]
        [NotNull]
        public static T[] __Shift<T>([NotNull] this T[] array)
        {
            return array.__Shift(out var void1);
        }

        /**
         * <summary>
         *     [DANGER]
         *     intなどの値方を処理するためにlengthが1以上であるかの確認を行っていません
         *     必ず呼び出し元で確認を行ってください
         * </summary>
         */
        [Obsolete("Use ArrayUtils.Shift instead")]
        [NotNull]
        public static T[] __Shift<T>([NotNull] this T[] array, [CanBeNull] out T item)
        {
            item = array[0];
            var tmpArray = new T[array.Length - 1];
            Array.Copy(array, 1, tmpArray, 0, array.Length - 1);
            return tmpArray;
        }

        [NotNull]
        public static T[] Pop<T>([NotNull] this T[] array)
        {
            return array.Pop(out var void1, out var void2);
        }

        [NotNull]
        public static T[] Pop<T>([NotNull] this T[] array, [CanBeNull] out T item)
        {
            return array.Pop(out item, out var void1);
        }

        [NotNull]
        public static T[] Pop<T>([NotNull] this T[] array, [CanBeNull] out T item, out bool success)
        {
            if (array.Length == 0)
            {
                item = default;
                success = false;
                return array;
            }

            item = array[array.Length - 1];
            var tmpArray = new T[array.Length - 1];
            Array.Copy(array, 0, tmpArray, 0, array.Length - 1);
            success = true;
            return tmpArray;
        }

        /**
         * <summary>
         *     [DANGER]
         *     intなどの値方を処理するためにlengthが1以上であるかの確認を行っていません
         *     必ず呼び出し元で確認を行ってください
         * </summary>
         */
        [Obsolete("Use ArrayUtils.Pop instead")]
        [NotNull]
        public static T[] __Pop<T>([NotNull] this T[] array, [CanBeNull] out T item)
        {
            item = array[array.Length - 1];
            var tmpArray = new T[array.Length - 1];
            Array.Copy(array, 0, tmpArray, 0, array.Length - 1);
            return tmpArray;
        }

        /**
         * <summary>
         *     removed: currentから削除された要素のcurrentのindex
         *     added: newArrayに追加された要素のnewArrayのindex
         * </summary>
         */
        public static void Diff<T>([NotNull] this T[] current, [NotNull] T[] newArray, out int[] removed,
            out int[] added)
        {
            removed = new int[current.Length];
            var removedIndex = 0;
            for (var i = 0; i < current.Length; i++)
            {
                if (newArray.Has(current[i])) continue;
                removed[removedIndex++] = i;
            }

            added = new int[newArray.Length];
            var addedIndex = 0;
            for (var i = 0; i < newArray.Length; i++)
            {
                if (current.Has(newArray[i])) continue;
                added[addedIndex++] = i;
            }

            removed = removed.Resize(removedIndex);
            added = added.Resize(addedIndex);
        }
        
        [NotNull]
        public static T[] Unshift<T>([NotNull] this T[] array, [CanBeNull] T item)
        {
            var tmpArray = new T[array.Length + 1];
            tmpArray[0] = item;
            Array.Copy(array, 0, tmpArray, 1, array.Length);
            return tmpArray;
        }
    }
}
