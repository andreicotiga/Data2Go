using Microsoft.EntityFrameworkCore;

namespace Data2Go.EntityFrameworkCore.Tests.Infrastructure
{
    public class ToGoDataContext : DbContext
    {
        public ToGoDataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ToGoOrderConfiguration());
            modelBuilder.ApplyConfiguration(new ToGoOrderItemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
