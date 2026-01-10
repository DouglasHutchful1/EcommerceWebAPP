using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string ShippingName { get; set; } = null!;
        public string ShippingEmail { get; set; } = null!;
        public string ShippingPhone { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        public int UserIdFk { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}