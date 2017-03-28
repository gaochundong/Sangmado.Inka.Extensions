using System.Collections.Generic;

namespace RockStone.Inka.Extensions
{
    public interface IQueue<T> : IEnumerable<T>, IReadOnlyCollection<T>
    {
        void Enqueue(T item);
        T Dequeue();
        T Peek();
        bool Contains(T item);
        void Clear();
    }
}
