using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data2Go.EntityFrameworkCore
{
    internal class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
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
