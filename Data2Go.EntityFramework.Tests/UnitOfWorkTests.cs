using System;
using System.Linq;
using System.Threading.Tasks;
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
            Assert.Equals(1, saved);

            var existing = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Count();
            Assert.Equals(1, existing);
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
            Assert.Equals(2, saved);

            var existing = _unitOfWork
                .GetRepository<ToGoOrderItem>()
                .Query()
                .Count();
            Assert.Equals(2, existing);
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
            Assert.Equals(order.Name, queryOrder.Name);
            Assert.Equals(order.CreatedDate, queryOrder.CreatedDate);
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

            Assert.Equals(2, bigCount);
        }

        [TestMethod]
        public async Task Delete_WhenDeletingEntity_Works()
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

            Assert.Equals(1, count);
        }
    }
}
