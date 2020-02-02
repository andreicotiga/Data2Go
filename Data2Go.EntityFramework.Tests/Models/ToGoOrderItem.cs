namespace Data2Go.EntityFramework.Tests.Models
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
