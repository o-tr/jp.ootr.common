using System;
using UnityEngine;

namespace jp.ootr.common
{
    public static class ArrayUtils
    {
        public const string PackageName = "jp.ootr.common.ArrayUtils";


        public static T[] Remove<T>(this T[] array, int index)
        {
            return array.Remove(index, out var _1);
        }

        public static T[] Remove<T>(this T[] array, int index, out T item)
        {
            if (index < 0)
            {
                index = array.Length + index;
            }
            
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

        public static T[] Append<T>(this T[] array, T item)
        {
            var tmpArray = new T[array.Length + 1];
            array.CopyTo(tmpArray, 0);
            tmpArray[array.Length] = item;
            return tmpArray;
        }

        public static T[] Replace<T>(this T[] array, T[] items, int index)
        {
            if (index < 0)
            {
                index = array.Length + index;
            }
            
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

        public static T[] Resize<T>(this T[] array, int targetLength)
        {
            if (array.Length == targetLength) return array;
            if (targetLength < 0)
            {
                Console.Warn($"ResizeArray: Target length is less than 0: {targetLength}", PackageName);
                return array;
            }

            var tmpArray = new T[targetLength];
            Array.Copy(array, 0, tmpArray, 0, Mathf.Min(array.Length, targetLength));
            return tmpArray;
        }


        public static T[] Insert<T>(this T[] array, T item, int index)
        {
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

        public static T[] Insert<T>(this T[] array, T[] items, int index)
        {
            return array.Insert(items, index, items.Length);
        }

        public static T[] Insert<T>(this T[] array, T[] items, int index, int itemsLength)
        {
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

        public static bool Has<T>(this T[] array, T item)
        {
            return array.Has(item, out var tmp);
        }

        public static bool Has<T>(this T[] array, T item, out int index)
        {
            index = Array.IndexOf(array, item);
            return index != -1;
        }

        public static T[] Merge<T>(this T[] array, T[] items, bool unique = false)
        {
            var tmpArray = new T[array.Length + items.Length];
            Array.Copy(array, 0, tmpArray, 0, array.Length);
            Array.Copy(items, 0, tmpArray, array.Length, items.Length);
            return unique ? tmpArray.Unique() : tmpArray;
        }

        public static T[] Unique<T>(this T[] array)
        {
            var tmpArray = new T[array.Length];
            var tmpIndex = 0;

            foreach (var item in array)
            {
                if (tmpArray.Has(item)) continue;
                tmpArray[tmpIndex++] = item;
            }

            return tmpArray.Resize(tmpIndex);
        }

        public static int[] IntUnique(this int[] array)
        {
            var tmpArray = new int[array.Length];
            var tmpIndex = 0;
            if (array.Has(0)) tmpIndex++;

            foreach (var item in array)
            {
                if (tmpArray.Has(item)) continue;
                tmpArray[tmpIndex++] = item;
            }

            return tmpArray.Resize(tmpIndex);
        }

        public static T[] __Shift<T>(this T[] array)
        {
            return array.__Shift(out var _void);
        }

        /**
         * <summary>
         *     [DANGER]
         *     intなどの値方を処理するためにlengthが1以上であるかの確認を行っていません
         *     必ず呼び出し元で確認を行ってください
         * </summary>
         */
        public static T[] __Shift<T>(this T[] array, out T item)
        {
            item = array[0];
            var tmpArray = new T[array.Length - 1];
            Array.Copy(array, 1, tmpArray, 0, array.Length - 1);
            return tmpArray;
        }

        /**
         * <summary>
         *     [DANGER]
         *     intなどの値方を処理するためにlengthが1以上であるかの確認を行っていません
         *     必ず呼び出し元で確認を行ってください
         * </summary>
         */
        public static T[] __Pop<T>(this T[] array, out T item)
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
        public static void Diff<T>(this T[] current, T[] newArray, out int[] removed, out int[] added)
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
    }
}
