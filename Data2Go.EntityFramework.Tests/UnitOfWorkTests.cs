using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data2Go.EntityFramework.Tests.Infrastructure;
using Data2Go.EntityFramework.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data2Go.EntityFramework.Tests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            var context = new ToGoDataContext(Effort.DbConnectionFactory.CreateTransient());
            _unitOfWork = context.ToGo();
        }

        [TestMethod]
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
            Assert.AreEqual(1, saved);

            var existing = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Count();
            Assert.AreEqual(1, existing);
        }

        [TestMethod]
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
            Assert.AreEqual(2, saved);

            var existing = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Count();
            Assert.AreEqual(2, existing);
        }

        [TestMethod]
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

            Assert.IsNotNull(queryOrder);
            Assert.AreEqual(order.Name, queryOrder.Name);
            Assert.AreEqual(order.CreatedDate, queryOrder.CreatedDate);
        }

        [TestMethod]
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

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Id);
            Assert.AreEqual("Beef Burger", found.Name);
        }

        [TestMethod]
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

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Id);
            Assert.AreEqual("Beef Burger", found.Name);
        }

        [TestMethod]
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

            Assert.AreEqual(2, bigCount);
        }

        [TestMethod]
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

            Assert.AreEqual(1, count);
        }

        [TestMethod]
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

            Assert.AreEqual(2, all.Count);

            _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .RemoveRange(all);

            await _unitOfWork.SaveAsync();

            var count = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query().Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
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

            Assert.IsNull(found);
        }
    }
}
