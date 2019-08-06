﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Data2Go.EntityFramework
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        protected readonly DbSet<T> DbSet;

        public ReadOnlyRepository(DbSet<T> dbSet)
        {
            DbSet = dbSet;
        }

        public T Find(params object[] key)
        {
            return DbSet.Find(key);
        }

        public Task<T> FindAsync(params object[] key)
        {
            return DbSet.FindAsync(key);
        }

        public virtual IQueryable<T> Query()
        {
            return DbSet.AsNoTracking();
        }
    }
}