using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Data2Go.EntityFrameworkCore.Tests.Infrastructure
{
    public class ToGoDataContext : DbContext
    {
        public ToGoDataContext() { }
        public ToGoDataContext(DbContextOptions options) : base(options) { }
        public DbSet<ToGoOrder> Orders { get; set; }
        public DbSet<ToGoOrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ToGoOrderConfiguration());
            modelBuilder.ApplyConfiguration(new ToGoOrderItemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
