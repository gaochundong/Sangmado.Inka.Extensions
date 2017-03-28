using System.Collections.Generic;

namespace RockStone.Inka.Extensions
{
    public interface IStack<T> : IEnumerable<T>, IReadOnlyCollection<T>
    {
        void Push(T item);
        T Pop();
        T Peek();
        bool Contains(T item);
        void Clear();
    }
}
