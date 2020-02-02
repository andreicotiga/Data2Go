using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Data2Go.EntityFramework.Tests.Models;

namespace Data2Go.EntityFramework.Tests.Infrastructure
{
    public class ToGoOrderItemConfiguration : EntityTypeConfiguration<ToGoOrderItem>
    {
        public void Configure(DbModelBuilder builder)
        {
            builder.Entity<ToGoOrderItem>()
                .HasKey(t => t.Id)
                .HasRequired(t => t.Order);
        }
    }
}
