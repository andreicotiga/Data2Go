using System;
using System.Threading.Tasks;

namespace Data2Go
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        Task<int> FlushAsync();

        Task<int> SaveAsync();
    }
}
