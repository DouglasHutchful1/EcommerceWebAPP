using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new();

        public void OnGet()
        {
            CartItems = new List<CartItem>
            {
                new CartItem { ProductName = "Wireless Headphones", Quantity = 2, Price = 49.99m, ImageUrl = "https://source.unsplash.com/100x100/?headphones" },
                new CartItem { ProductName = "Smart Watch", Quantity = 1, Price = 79.99m, ImageUrl = "https://source.unsplash.com/100x100/?watch" }
            };
        }
    }

    public class CartItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
