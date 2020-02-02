using System.Collections.Generic;

namespace Data2Go
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T: class
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);

        void Remove(params object[] key);

        void RemoveRange(IEnumerable<T> entities);
    }
}
