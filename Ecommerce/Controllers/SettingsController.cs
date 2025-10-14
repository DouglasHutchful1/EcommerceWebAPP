using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

public class SettingsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}