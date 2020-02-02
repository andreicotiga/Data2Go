using System;
using System.Collections.Generic;

namespace Data2Go.EntityFrameworkCore.Tests.Infrastructure
{
    public class ToGoOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<ToGoOrderItem> OrderItems { get; set; }
    }
}
