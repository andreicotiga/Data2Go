using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Data2Go.EntityFrameworkCore
{
    internal class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : class
    {
        public Repository(DbSet<T> dbSet) : base(dbSet)
        {
        }

        public override IQueryable<T> Query()
        {
            return DbSet;
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Remove(params object[] key)
        {
            var entity = DbSet.Find(key);
            if (entity != null)
            {
                Remove(entity);
            }
        }
    }
}
