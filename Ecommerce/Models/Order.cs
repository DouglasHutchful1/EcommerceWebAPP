using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    
    [Table("Order")] 
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string? ShippingAddress { get; set; } 
        public string? Status { get; set; } 
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        public int UserIdFk { get; set; }
    }
}
