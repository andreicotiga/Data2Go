using System;
using System.Collections.Generic;

namespace Data2Go.EntityFramework.Tests.Models
{
    public class ToGoOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ToGoOrderItem> OrderItems { get; set; }
    }
}
