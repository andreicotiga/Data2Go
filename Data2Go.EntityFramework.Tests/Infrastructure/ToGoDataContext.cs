using System.Data.Common;
using System.Data.Entity;
using Data2Go.EntityFramework.Tests.Models;

namespace Data2Go.EntityFramework.Tests.Infrastructure
{
    public class ToGoDataContext : DbContext
    {
        internal ToGoDataContext(DbConnection connection)
            : base(connection, true) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ToGoOrderConfiguration());
            modelBuilder.Configurations.Add(new ToGoOrderItemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
