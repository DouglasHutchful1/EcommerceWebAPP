using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    [Table("CartItem")] 
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? UserId { get; set; }

        public Product? Product { get; set; } 
    }
    public class CartModel
    {
        public List<CartItem> CartItems { get; set; }
    }
}
