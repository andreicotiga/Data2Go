using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data2Go.EntityFrameworkCore
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _dbContext.Database.BeginTransaction();
            }
        }

        public IReadOnlyRepository<T> GetReadOnlyRepository<T>() where T : class
        {
            return new ReadOnlyRepository<T>(_dbContext.Set<T>());
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_dbContext.Set<T>());
        }

        public Task<int> FlushAsync()
        {
            BeginTransaction();

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> SaveAsync()
        {
            var count = _dbContext.SaveChangesAsync();

            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;

            return count;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}
