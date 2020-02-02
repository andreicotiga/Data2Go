using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data2Go.EntityFrameworkCore.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Data2Go.EntityFrameworkCore.Tests
{
    public class UnitOfWorkTests : InMemoryDbTests
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _unitOfWork = GetDbContext().ToGo();
        }

        [Fact]
        public async Task Add_WhenAddingSingleEntity_Works()
        {
            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Add(new ToGoOrderItem
                {
                    Name = "Beef Burger",
                    Size = Size.Big
                });

            var saved = await _unitOfWork.SaveAsync();
            Assert.Equal(1, saved);

            var existing = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Count();
            Assert.Equal(1, existing);
        }

        [Fact]
        public async Task Add_WhenAddingMultipleEntities_Works()
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


            var saved = await _unitOfWork.SaveAsync();
            Assert.Equal(2, saved);

            var existing = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Count();
            Assert.Equal(2, existing);
        }

        [Fact]
        public async Task Add_WhenAddingNavigationEntities_Works()
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
        public async Task Find_WhenSearchingEntity_Works()
        {
            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Add(new ToGoOrderItem
                {
                    Id = 1,
                    Name = "Beef Burger",
                    Size = Size.Big
                });

            var saved = await _unitOfWork.SaveAsync();

            var found = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Find(1);

            Assert.NotNull(found);
            Assert.Equal(1, found.Id);
            Assert.Equal("Beef Burger", found.Name);
        }

        [Fact]
        public async Task FindAsync_WhenSearchingEntity_Works()
        {
            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Add(new ToGoOrderItem
                {
                    Id = 1,
                    Name = "Beef Burger",
                    Size = Size.Big
                });

            var saved = await _unitOfWork.SaveAsync();

            var found = await _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .FindAsync(CancellationToken.None, 1);

            Assert.NotNull(found);
            Assert.Equal(1, found.Id);
            Assert.Equal("Beef Burger", found.Name);
        }

        [Fact]
        public async Task Update_WhenUpdatingEntityProperty_Works()
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
        public async Task Remove_WhenRemovingSingleEntity_Works()
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

        [Fact]
        public async Task Remove_WhenRemovingMultipleEntities_Works()
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

            var all = await _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Where(x => x.Name.Contains("Burger"))
                .ToListAsync();

            Assert.Equal(2, all.Count);

            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .RemoveRange(all);

            await _unitOfWork.SaveAsync();

            var count = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query().Count();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Remove_WhenRemovingEntityById_Works()
        {
            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .AddRange(new[]
                {
                    new ToGoOrderItem
                    {
                        Id = 1,
                        Name = "Beef Burger",
                        Size = Size.Big
                    },
                    new ToGoOrderItem
                    {
                        Id = 2,
                        Name = "Chicken Burger",
                        Size = Size.Small
                    }
                });

            await _unitOfWork.SaveAsync();

            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Remove(1);

            await _unitOfWork.SaveAsync();

            var found = await _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .FindAsync(CancellationToken.None, 1);

            Assert.Null(found);
        }
    }
}
