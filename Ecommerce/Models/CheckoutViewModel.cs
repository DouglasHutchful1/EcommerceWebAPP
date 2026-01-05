namespace Ecommerce.Models;

public class CheckoutViewModel
{
    public List<CartItem> CartItems { get; set; } = new();

    // Customer info
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";

    // Totals
    public int TotalItems =>
        CartItems.Sum(x => x.Quantity);

    public decimal TotalAmount =>
        CartItems.Sum(x => x.Product!.Price * x.Quantity);
}