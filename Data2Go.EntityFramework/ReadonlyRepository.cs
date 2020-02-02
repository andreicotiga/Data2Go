using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data2Go.EntityFramework
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

        public ValueTask<T> FindAsync(object[] key, CancellationToken cancellationToken)
        {
            return new ValueTask<T>(DbSet.FindAsync(cancellationToken, key));
        }

        public virtual IQueryable<T> Query()
        {
            return DbSet.AsNoTracking();
        }
    }
}
