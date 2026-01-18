using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers;

public class CheckoutController(EcommerceDbContext _db,ILogger<CheckoutController> _logger) : Controller
{
    // GET
   
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (userId <= 0)
        {
            TempData["LoginRequired"] = "Please login to continue to checkout.";
            return RedirectToAction("Index", "Home");
        }
        var model = new CheckoutViewModel
        {
            CartItems = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync()
        };

        if (!model.CartItems.Any())
            return RedirectToAction("Index", "Cart");

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
    {
        
        try
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var cartItems = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            var order = new Order
            {
                UserIdFk = userId.Value,

                ShippingName = model.FullName,
                ShippingEmail = model.Email,
                ShippingPhone = model.Phone,
                ShippingAddress = model.Address,

                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(x =>
                    x.Product!.Price * x.Quantity
                )
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            var orderItems = cartItems.Select(item => new OrderItem
            {
                OrderId = order.Id,
                ProductIdfk = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Product!.Price
            });

            _db.OrderItems.AddRange(orderItems);

            _db.CartItems.RemoveRange(cartItems);

            await _db.SaveChangesAsync();

            return RedirectToAction("Success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error placing order");
            ModelState.AddModelError("", "Unable to place order.");
            return View("Index", model);
        }
    }



    public IActionResult Success()
    {
        return View();
    }
}