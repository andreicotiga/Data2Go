using Data2Go.EntityFrameworkCore.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Data2Go.EntityFrameworkCore.Tests
{
    public class BaseTest
    {
        protected ToGoDataContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<ToGoDataContext>();
            builder.UseInMemoryDatabase("ToGo");

            var personDataContext = new ToGoDataContext(builder.Options);
            personDataContext.Database.EnsureDeleted();
            personDataContext.Database.EnsureCreated();

            return personDataContext;
        }
    }
}
