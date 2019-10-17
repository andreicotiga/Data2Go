using System;
using System.Data.Entity;

namespace Data2Go.EntityFramework
{
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Creates a new unit of work for the provided context
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A new unit of work</returns>
        public static IUnitOfWork ToGo(this DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new UnitOfWork(context);
        }
    }
}
