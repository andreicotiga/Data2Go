using Data2Go.EntityFrameworkCore.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data2Go.EntityFrameworkCore.Tests.Infrastructure
{
    public class ToGoOrderItemConfiguration : IEntityTypeConfiguration<ToGoOrderItem>
    {
        public void Configure(EntityTypeBuilder<ToGoOrderItem> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.HasOne(t => t.Order);
        }
    }
}
