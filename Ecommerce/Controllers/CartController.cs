using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers;

public class CartController(EcommerceDbContext _db,ILogger<CartController> _logger) : Controller
{
    

public async Task<IActionResult> Index()
{
    int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

    var model = new CartModel
    {
        CartItems = await _db.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync()
    };

    return View(model);
}

    // Cart/Get
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var items = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    c.Id,
                    c.ProductId,
                    c.Quantity,
                    name = c.Product!.Name,
                    price = c.Product.Price,
                    image = c.Product.ImageUrl
                })
                .ToListAsync();

            return Json(new
            {
                items,
                total = items.Sum(x => x.price * x.Quantity)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching cart items");
            return StatusCode(500, new { message = "An error occurred while fetching cart items.", error = ex.Message });
        }
    }

    // Cart/Add
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CartItem input)
    {
        try
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var existing = await _db.CartItems
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.ProductId == input.ProductId);

            if (existing != null)
            {
                existing.Quantity += input.Quantity;
            }
            else
            {
                _db.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    ProductId = input.ProductId,
                    Quantity = input.Quantity
                });
            }

            await _db.SaveChangesAsync();

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding item to cart");
            return StatusCode(500, new { message = "An error occurred while adding item to cart.", error = ex.Message });
        }
    }

    //  Cart/Update
    [HttpPost]
    public async Task<IActionResult> Update(int productId, int qty)
    {
        try
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var item = await _db.CartItems
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        if (item == null)
            return NotFound();

        item.Quantity = Math.Max(1, qty);
        await _db.SaveChangesAsync();

        return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item");
            return StatusCode(500, new { message = "An error occurred while updating cart item.", error = ex.Message });
        }
    }

    // Cart/Remove
    [HttpPost]
    public async Task<IActionResult> Remove(int productId)
    {
        try
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var item = await _db.CartItems
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

            if (item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cart item");
            return StatusCode(500, new { message = "An error occurred while removing cart item.", error = ex.Message });
        }
    }
}
