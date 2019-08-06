using System.Linq;
using System.Threading.Tasks;

namespace Data2Go
{
    public interface IReadOnlyRepository<T> where T : class
    {
        T Find(params object[] key);

        Task<T> FindAsync(params object[] key);

        IQueryable<T> Query();
    }
}
