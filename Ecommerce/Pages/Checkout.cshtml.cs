using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Pages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public string BillingName { get; set; }
        [BindProperty]
        public string BillingAddress { get; set; }
        [BindProperty]
        public string BillingCity { get; set; }
        [BindProperty]
        public string BillingPhone { get; set; }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public void OnGet()
        {
            CartItems = new List<CartItem>
            {
                new CartItem { ProductName = "Wireless Headphones", Quantity = 2, Price = 49.99m },
                new CartItem { ProductName = "Smart Watch", Quantity = 1, Price = 79.99m }
            };

            TotalAmount = CartItems.Sum(x => x.Quantity * x.Price);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // will make som changs here
            return RedirectToPage("/ThankYou");
        }
    }

}
