using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Data2Go.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private DbContextTransaction _transaction;

        public EntityFrameworkUnitOfWork(DbContext dbContext)
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
