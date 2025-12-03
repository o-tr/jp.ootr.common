using System;
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;

namespace jp.ootr.common
{
    /// <summary>
    /// UdonSharp環境で動作するList<T>の闇クラス実装
    /// 参考: https://power-of-tech.hatenablog.com/entry/2024/06/12/191828
    /// </summary>
    public class UList<T> : UdonSharpBehaviour
    {
        private const string PackageName = "jp.ootr.common.UList";

        /// <summary>
        /// 疑似コンストラクタ：新しいUListインスタンスを作成
        /// </summary>
        /// <param name="initialCapacity">初期容量（デフォルト: 4）</param>
        /// <returns>UListインスタンス</returns>
        public static UList<T> New(int initialCapacity = 4)
        {
            if (initialCapacity < 0)
            {
                Console.Warn($"UList.New: Initial capacity is less than 0: {initialCapacity}, using default value 4",
                    PackageName);
                initialCapacity = 4;
            }

            var items = new T[initialCapacity];
            var count = 0;
            var capacity = initialCapacity;

            var buff = new object[]
            {
                items,
                count,
                capacity
            };
            return (UList<T>)(object)(buff);
        }
    }

    /// <summary>
    /// UList<T>の拡張メソッドクラス
    /// </summary>
    public static class UListExt
    {
        private const string PackageName = "jp.ootr.common.UList";

        // フィールド読み取りメソッド

        /// <summary>
        /// 内部配列を取得
        /// </summary>
        [NotNull]
        public static T[] Items<T>([NotNull] this UList<T> list)
        {
            return (T[])(((object[])(object)list)[0]);
        }

        /// <summary>
        /// 現在の要素数を取得
        /// </summary>
        public static int Count<T>([NotNull] this UList<T> list)
        {
            return (int)(((object[])(object)list)[1]);
        }

        /// <summary>
        /// 容量を取得
        /// </summary>
        public static int Capacity<T>([NotNull] this UList<T> list)
        {
            return (int)(((object[])(object)list)[2]);
        }

        /// <summary>
        /// 指定インデックスの要素を取得
        /// </summary>
        [CanBeNull]
        public static T Get<T>([NotNull] this UList<T> list, int index)
        {
            var count = list.Count<T>();
            if (index < 0 || index >= count)
            {
                Console.Warn($"UList.Get: Index out of range: {index}, count: {count}", PackageName);
                return default;
            }

            var items = list.Items<T>();
            return items[index];
        }

        // フィールド書き込みメソッド

        /// <summary>
        /// 内部配列を設定
        /// </summary>
        public static void SetItems<T>([NotNull] this UList<T> list, [NotNull] T[] items)
        {
            ((object[])(object)list)[0] = items;
        }

        /// <summary>
        /// 要素数を設定
        /// </summary>
        public static void SetCount<T>([NotNull] this UList<T> list, int count)
        {
            if (count < 0)
            {
                Console.Warn($"UList.SetCount: Count is less than 0: {count}", PackageName);
                count = 0;
            }

            ((object[])(object)list)[1] = count;
        }

        /// <summary>
        /// 容量を設定
        /// </summary>
        public static void SetCapacity<T>([NotNull] this UList<T> list, int capacity)
        {
            if (capacity < 0)
            {
                Console.Warn($"UList.SetCapacity: Capacity is less than 0: {capacity}", PackageName);
                capacity = 0;
            }

            ((object[])(object)list)[2] = capacity;
        }

        /// <summary>
        /// 指定インデックスの要素を設定
        /// </summary>
        public static void Set<T>([NotNull] this UList<T> list, int index, [CanBeNull] T item)
        {
            var count = list.Count<T>();
            if (index < 0 || index >= count)
            {
                Console.Warn($"UList.Set: Index out of range: {index}, count: {count}", PackageName);
                return;
            }

            var items = list.Items<T>();
            items[index] = item;
        }

        // リスト操作メソッド

        /// <summary>
        /// 容量を拡張（必要に応じて2倍に拡張）
        /// </summary>
        private static void EnsureCapacity<T>([NotNull] this UList<T> list, int minCapacity)
        {
            var currentCapacity = list.Capacity<T>();
            if (currentCapacity >= minCapacity) return;

            var newCapacity = Mathf.Max(currentCapacity * 2, minCapacity);
            var items = list.Items<T>();
            var count = list.Count<T>();

            var newItems = new T[newCapacity];
            if (items != null && count > 0)
            {
                Array.Copy(items, 0, newItems, 0, count);
            }

            list.SetItems<T>(newItems);
            list.SetCapacity<T>(newCapacity);
        }

        /// <summary>
        /// 要素を末尾に追加
        /// </summary>
        public static void Add<T>([NotNull] this UList<T> list, [CanBeNull] T item)
        {
            var count = list.Count<T>();
            list.EnsureCapacity<T>(count + 1);

            var items = list.Items<T>();
            items[count] = item;
            list.SetCount<T>(count + 1);
        }

        /// <summary>
        /// 指定要素を削除（最初の一致のみ）
        /// </summary>
        /// <returns>削除に成功した場合はtrue、要素が見つからない場合はfalse</returns>
        public static bool Remove<T>([NotNull] this UList<T> list, [CanBeNull] T item)
        {
            var index = list.IndexOf<T>(item);
            if (index == -1) return false;

            list.RemoveAt<T>(index);
            return true;
        }

        /// <summary>
        /// 指定インデックスの要素を削除
        /// </summary>
        public static void RemoveAt<T>([NotNull] this UList<T> list, int index)
        {
            var count = list.Count<T>();
            if (index < 0 || index >= count)
            {
                Console.Warn($"UList.RemoveAt: Index out of range: {index}, count: {count}", PackageName);
                return;
            }

            var items = list.Items<T>();
            if (index < count - 1)
            {
                Array.Copy(items, index + 1, items, index, count - index - 1);
            }

            items[count - 1] = default;
            list.SetCount<T>(count - 1);
        }

        /// <summary>
        /// 要素の存在確認
        /// </summary>
        /// <returns>要素が存在する場合はtrue、存在しない場合はfalse</returns>
        public static bool Contains<T>([NotNull] this UList<T> list, [CanBeNull] T item)
        {
            return list.IndexOf<T>(item) != -1;
        }

        /// <summary>
        /// 要素のインデックス取得
        /// </summary>
        /// <returns>要素のインデックス、見つからない場合は-1</returns>
        public static int IndexOf<T>([NotNull] this UList<T> list, [CanBeNull] T item)
        {
            var items = list.Items<T>();
            var count = list.Count<T>();

            if (item == null)
            {
                for (var i = 0; i < count; i++)
                {
                    if (items[i] == null) return i;
                }
            }
            else
            {
                for (var i = 0; i < count; i++)
                {
                    if (item.Equals(items[i])) return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 全要素を削除
        /// </summary>
        public static void Clear<T>([NotNull] this UList<T> list)
        {
            var items = list.Items<T>();
            var count = list.Count<T>();

            // 参照をクリア（ガベージコレクションを助ける）
            for (var i = 0; i < count; i++)
            {
                items[i] = default;
            }

            list.SetCount<T>(0);
        }

        /// <summary>
        /// 指定位置に要素を挿入
        /// </summary>
        public static void Insert<T>([NotNull] this UList<T> list, int index, [CanBeNull] T item)
        {
            var count = list.Count<T>();
            if (index < 0 || index > count)
            {
                Console.Warn($"UList.Insert: Index out of range: {index}, count: {count}", PackageName);
                return;
            }

            list.EnsureCapacity<T>(count + 1);

            var items = list.Items<T>();
            if (index < count)
            {
                Array.Copy(items, index, items, index + 1, count - index);
            }

            items[index] = item;
            list.SetCount<T>(count + 1);
        }
    }
}

