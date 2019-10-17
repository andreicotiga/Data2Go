using Data2Go.EntityFrameworkCore.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Data2Go.EntityFrameworkCore.Tests
{
    public class InMemoryDbTests
    {
        protected ToGoDataContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<ToGoDataContext>();
            builder.UseInMemoryDatabase("ToGo");

            var toGoDataContext = new ToGoDataContext(builder.Options);
            toGoDataContext.Database.EnsureDeleted();
            toGoDataContext.Database.EnsureCreated();

            return toGoDataContext;
        }
    }
}
