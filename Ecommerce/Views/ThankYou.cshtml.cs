using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce.Pages
{
    public class ThankYouModel : PageModel
    {
        public string OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }

        public void OnGet()
        {
            OrderId = "5";  
            TotalAmount = 99.99m;  
            ShippingAddress = "123 USA";  
        }
    }
}
