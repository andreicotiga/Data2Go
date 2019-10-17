using System;
using System.Linq;
using System.Threading.Tasks;
using Data2Go.EntityFrameworkCore.Tests.Infrastructure;
using Xunit;

namespace Data2Go.EntityFrameworkCore.Tests
{
    public class RepositoryTests : InMemoryDbTests
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryTests()
        {
            _unitOfWork = GetDbContext().ToGo();
        }

        [Fact]
        public async Task Insert_Works()
        {
            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Add(new ToGoOrderItem
                {
                    Name = "Beef Burger",
                    Size = Size.Big
                });

            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Add(new ToGoOrderItem
                {
                    Name = "Chicken Burger",
                    Size = Size.Small
                });

            var saved = await _unitOfWork.SaveAsync();
            Assert.Equal(2, saved);

            var existing = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Count();
            Assert.Equal(2, existing);
        }

        [Fact]
        public async Task InsertWithNavigation_Works()
        {
            var order = new ToGoOrder
            {
                CreatedDate = DateTime.Now,
                Name = "The 1st Order",
            };

            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .AddRange(new[]
                {
                    new ToGoOrderItem
                    {
                        Name = "Beef Burger",
                        Size = Size.Big,
                        Order = order
                    },
                    new ToGoOrderItem
                    {
                        Name = "Chicken Burger",
                        Size = Size.Small,
                        Order = order
                    }
                });

            await _unitOfWork.SaveAsync();

            var queryOrder = _unitOfWork
                .GetRepository<ToGoOrder>()
                .Query()
                .SingleOrDefault();

            Assert.NotNull(queryOrder);
            Assert.Equal(order.Name, queryOrder.Name);
            Assert.Equal(order.CreatedDate, queryOrder.CreatedDate);
        }

        [Fact]
        public async Task Update_Works()
        {
            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .AddRange(new[]
                {
                    new ToGoOrderItem
                    {
                        Name = "Beef Burger",
                        Size = Size.Small
                    },
                    new ToGoOrderItem
                    {
                        Name = "Chicken Burger",
                        Size = Size.Small
                    }
                });

            await _unitOfWork.SaveAsync();

            var small = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Where(i => i.Size == Size.Small);

            foreach (var s in small)
            {
                s.Size = Size.Big;
            }

            await _unitOfWork.SaveAsync();

            var bigCount = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Count(i => i.Size == Size.Big);

            Assert.Equal(2, bigCount);
        }

        [Fact]
        public async Task Delete_Works()
        {
            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .AddRange(new[]
                {
                    new ToGoOrderItem
                    {
                        Name = "Beef Burger",
                        Size = Size.Big
                    },
                    new ToGoOrderItem
                    {
                        Name = "Chicken Burger",
                        Size = Size.Small
                    }
                });

            await _unitOfWork.SaveAsync();

            var big = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Single(i => i.Size == Size.Big);

            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Remove(big);

            await _unitOfWork.SaveAsync();

            var count = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query().Count();

            Assert.Equal(1, count);
        }
    }
}
