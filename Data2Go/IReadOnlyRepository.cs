using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data2Go
{
    public interface IReadOnlyRepository<T> where T : class
    {
        T Find(params object[] key);

        ValueTask<T> FindAsync(CancellationToken cancellationToken, params object[] key);

        IQueryable<T> Query();
    }
}
