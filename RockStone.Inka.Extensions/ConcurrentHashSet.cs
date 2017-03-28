using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RockStone.Inka.Extensions
{
    public class ConcurrentHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly object PRESENT = new object();
        private readonly ConcurrentDictionary<T, object> _dict = new ConcurrentDictionary<T, object>();

        public IEnumerator<T> GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsEmpty
        {
            get { return _dict.IsEmpty; }
        }

        void ICollection<T>.Add(T item)
        {
            if (!Add(item))
                throw new ArgumentException("Item already exists in set.");
        }

        public bool Add(T item)
        {
            return TryAdd(item);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(T item)
        {
            return _dict.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _dict.Keys.CopyTo(array, arrayIndex);
        }

        public T[] ToArray()
        {
            return _dict.Keys.ToArray();
        }

        public bool TryAdd(T item)
        {
            return _dict.TryAdd(item, PRESENT);
        }

        public bool TryRemove(T item)
        {
            object donotcare;
            return _dict.TryRemove(item, out donotcare);
        }

        public bool Remove(T item)
        {
            return TryRemove(item);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            foreach (var item in other)
                TryRemove(item);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            var enumerable = other as IList<T> ?? other.ToArray();
            foreach (var item in this)
            {
                if (!enumerable.Contains(item))
                    TryRemove(item);
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            var enumerable = other as IList<T> ?? other.ToArray();
            return this.Count != enumerable.Count() && IsSubsetOf(enumerable);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            var enumerable = other as IList<T> ?? other.ToArray();
            return this.Count != enumerable.Count() && IsSupersetOf(enumerable);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            var enumerable = other as IList<T> ?? other.ToArray();
            return this.AsParallel().All(enumerable.Contains);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            return other.AsParallel().All(Contains);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            return other.AsParallel().Any(Contains);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            var enumerable = other as IList<T> ?? other.ToArray();
            return this.Count == enumerable.Count() && enumerable.AsParallel().All(Contains);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            List<T> existsInOther = new List<T>();
            List<T> nonExistsInThis = new List<T>();

            foreach (var item in this)
            {
                if (other.Contains(item))
                    existsInOther.Add(item);
            }

            foreach (var item in other)
            {
                if (!this.Contains(item))
                    nonExistsInThis.Add(item);
            }

            ExceptWith(existsInOther);
            UnionWith(nonExistsInThis);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            Guard.ArgumentNotNull(other, "other");

            foreach (var item in other)
                TryAdd(item);
        }
    }
}
