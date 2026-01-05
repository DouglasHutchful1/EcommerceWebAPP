using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

public class ProductController(EcommerceDbContext _db,ILogger<ProductController> _logger) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    //  fetch products
    [HttpGet]
    public IActionResult GetProducts(string? search, string? category, string? sort)
    {
        try
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            query = sort switch
            {
                "low-high" => query.OrderBy(p => p.Price),
                "high-low" => query.OrderByDescending(p => p.Price),
                _ => query.OrderByDescending(p => p.CreationDate)
            };

            var products = query.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                category = p.Category,
                price = p.Price,
                image = p.ImageUrl
            }).ToList();

            return Json(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products");
            return StatusCode(500, new { message = "An error occurred while fetching products.", error = ex.Message });
        }
    }
}