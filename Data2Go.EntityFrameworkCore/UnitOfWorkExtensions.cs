using System;
using Microsoft.EntityFrameworkCore;

namespace Data2Go.EntityFrameworkCore
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
