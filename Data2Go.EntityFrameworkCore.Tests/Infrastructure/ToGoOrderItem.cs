using System;
using System.Collections.Generic;
using System.Text;

namespace Data2Go.EntityFrameworkCore.Tests.Infrastructure
{
    public class ToGoOrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Size Size { get; set; }
        public ToGoOrder Order { get; set; }
    }

    public enum Size
    {
        Small,
        Big
    }
}
