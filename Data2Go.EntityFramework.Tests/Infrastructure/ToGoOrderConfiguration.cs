using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Data2Go.EntityFramework.Tests.Models;

namespace Data2Go.EntityFramework.Tests.Infrastructure
{
    internal class ToGoOrderConfiguration : EntityTypeConfiguration<ToGoOrder>
    {
        public void Configure(DbModelBuilder builder)
        {
            builder.Entity<ToGoOrder>()
                .HasKey(o => o.Id)
                .HasMany(t => t.OrderItems);
        }
    }
}
