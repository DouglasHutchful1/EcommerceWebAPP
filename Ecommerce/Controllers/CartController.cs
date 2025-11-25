using Ecommerce.Models;
using Ecommerce.Pages;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

public class CartController : Controller
{
    public IActionResult Index()
    {
        var model = new CartModel
        {
            CartItems = new List<CartItem>()
        };
        

        return View();
    }
}
