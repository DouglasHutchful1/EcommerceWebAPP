using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class OrdersController(EcommerceDbContext _db,ILogger<OrdersController> _logger) : Controller
    {
       
        // Orders
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                    return RedirectToAction("Index", "Home");

                var orders = await _db.Orders
                    .Where(o => o.UserIdFk == userId.Value)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for user.");
                return StatusCode(500, "An error occurred while fetching your orders.");
            }
        }

        // /Orders/Details/
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                    return RedirectToAction("Index", "Home");

                var order = await _db.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o =>
                        o.Id == id &&
                        o.UserIdFk == userId.Value);

                if (order == null)
                    return NotFound();

                return View(order);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order details for order ID {OrderId}.", id);
                return StatusCode(500, "An error occurred while fetching the order details.");
            }
        }
    }
}