using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualisation.Utils
{
    public sealed class OrderedList<T, TKey> : ICollection<T> where TKey : IComparable<TKey>
    {
        private readonly Func<T, TKey> keyExtractor;
        private IDictionary<TKey, List<T>> items = new Dictionary<TKey, List<T>>();

        public OrderedList(Func<T, TKey> keyExtractor)
        {
            if (keyExtractor == null)
                throw new ArgumentNullException(nameof(keyExtractor));

            this.keyExtractor = keyExtractor;
        }

        public IEnumerator<T> GetEnumerator() => AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<T> AsEnumerable(bool orderByDescending = false)
        {
            var enumerable = items.SelectMany(x => x.Value);
            return orderByDescending
                ? enumerable.OrderByDescending(keyExtractor)
                : enumerable.OrderBy(keyExtractor);
        }

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var key = KeyOf(item);
            if (!items.TryGetValue(key, out var list))
                items[key] = list = new List<T>();
            list.Add(item);

            Count++;
        }

        public bool Contains(T item) =>
            item != null && items.TryGetValue(KeyOf(item), out var list) && list.Contains(item);

        public bool Remove(T item) => item != null && items.TryGetValue(KeyOf(item), out var list) && list.Remove(item);

        public void Clear()
        {
            Count = 0;
            items.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), "Target array is null");

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (Count > array.Length - arrayIndex)
                throw new ArgumentException("Number of elements in source collection is greater than available space",
                    nameof(array));

            AsEnumerable().ToArray().CopyTo(array, arrayIndex);
        }

        public int Count { get; private set; }
        public bool IsReadOnly => false;

        private TKey KeyOf(T first) => keyExtractor.Invoke(first);
    }
}