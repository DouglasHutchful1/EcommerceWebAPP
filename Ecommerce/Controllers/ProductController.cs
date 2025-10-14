using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}