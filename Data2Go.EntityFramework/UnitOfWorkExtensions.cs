using System;
using System.Data.Entity;

namespace Data2Go.EntityFramework
{
    public static class UnitOfWorkExtensions
    {
        public static IUnitOfWork ToGo(this DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }

            return new UnitOfWork(context);
        }
    }
}
