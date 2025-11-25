namespace Ecommerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Product? Product { get; set; } 
    }
    public class CartModel
    {
        public List<CartItem> CartItems { get; set; }
    }
}
