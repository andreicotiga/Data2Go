using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data2Go
{
    public interface IReadOnlyRepository<T> where T : class
    {
        T Find(params object[] key);

        ValueTask<T> FindAsync(object[] key, CancellationToken cancellationToken);

        IQueryable<T> Query();
    }
}
