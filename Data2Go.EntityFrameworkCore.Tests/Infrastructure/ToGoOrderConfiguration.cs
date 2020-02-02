using Data2Go.EntityFrameworkCore.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data2Go.EntityFrameworkCore.Tests.Infrastructure
{
    internal class ToGoOrderConfiguration : IEntityTypeConfiguration<ToGoOrder>
    {
        public void Configure(EntityTypeBuilder<ToGoOrder> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.HasMany(t => t.OrderItems);
        }
    }
}
