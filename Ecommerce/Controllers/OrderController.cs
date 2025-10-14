using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

public class OrderController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}