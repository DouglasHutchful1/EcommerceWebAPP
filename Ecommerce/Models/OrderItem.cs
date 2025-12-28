using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    [Table("OrderItem")] 

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductIdfk { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
