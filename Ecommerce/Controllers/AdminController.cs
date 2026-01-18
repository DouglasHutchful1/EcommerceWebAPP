using Ecommerce.Data;
using Ecommerce.Helpers;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers;

public class AdminController(EcommerceDbContext _db,ILogger<AdminController> _logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Products()
    {
        return View();
    } 
    public IActionResult Orders()
    {
        return View();
    }
    public IActionResult Users()
    {
        return View();
    }

    
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _db.Products
                .Where(p => p.Active)
                .OrderByDescending(p => p.CreationDate)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Category,
                    p.Price,
                    p.Stock,
                    p.ImageUrl
                })
                .ToListAsync();

            return Json(products);
        }catch(Exception ex)
        {
            _logger.LogError(ex, "Error fetching products");

            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(Product product, IFormFile Image)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return BadRequest(new { success = false, message = "Product name is required" });

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized();

            // Image upload
            if (Image != null && Image.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products");
                Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Image.CopyToAsync(stream);

                product.ImageUrl = "/uploads/products/" + fileName;
            }

            product.Active = true;
            product.CreationDate = DateTime.UtcNow;
            product.EditedDate = DateTime.UtcNow;
            product.CreatedBy = userId.Value;
            product.EditedBy = userId.Value;

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }

    [HttpGet]
    public IActionResult GetProduct(int id)
    {
        try
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            return Json(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products");

            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
        
    }

    
    [HttpPost]
    public async Task<IActionResult> Update(Product product, IFormFile Image)
    {
        try
        {
            var existing = await _db.Products.FindAsync(product.Id);
            if (existing == null)
                return Json(new { success = false, message = "Product not found" });

            existing.Name = product.Name;
            existing.Category = product.Category;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.EditedDate = DateTime.UtcNow;
            existing.EditedBy = HttpContext.Session.GetInt32("UserId") ?? 0;

            // Replace image ONLY if new one is uploaded
            if (Image != null && Image.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products");
                Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Image.CopyToAsync(stream);

                existing.ImageUrl = "/uploads/products/" + fileName;
            }

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }

    
    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return Json(new { success = false, message = "Product not found" });

            product.Active = false;
            product.EditedDate = DateTime.UtcNow;
            product.EditedBy = HttpContext.Session.GetInt32("UserId") ?? 0;

            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting");

            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }
    
      // get all users
      
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _db.User
                .OrderBy(u => u.Username)
                .Select(u => new
                {
                    u.Id,
                    u.Firstname,
                    u.Lastname,
                    u.Active,
                    u.Username,
                    u.Email,
                    u.UserType,
                    u.CreationDate
                })
                .ToListAsync();

            return Json(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching users");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }

    // get user by ID
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _db.User.FindAsync(id);
            if (user == null) return NotFound(new { success = false, message = "User not found" });

            return Json(new
            {
                user.Id,
                user.Firstname,
                user.Lastname,
                user.Username,
                user.Email,
                user.UserType,
                user.Active,
                user.CreationDate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error Occured");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }

    // add user
    public async Task<IActionResult> AddUser([FromBody] User requestdto)
    {
        if (string.IsNullOrWhiteSpace(requestdto.Username))
            return BadRequest(new { success = false, message = "Username is required" });

        try
        {
            var user = new User
            {
                Firstname = requestdto.Firstname,
                Lastname = requestdto.Lastname,
                Email = requestdto.Email,
                Username = requestdto.Username,
                UserType = requestdto.UserType,
                Password = PasswordHelper.HashPassword(requestdto.Password),
                Active = true,
                CreationDate = DateTime.UtcNow
            };
            _db.User.Add(user);
            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding user");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }

    // update user
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        try
        {
            var existing = await _db.User.FindAsync(user.Id);
            if (existing == null) return NotFound(new { success = false, message = "User not found" });

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.UserType = user.UserType;
            if (!string.IsNullOrEmpty(user.Password))
            {
                existing.Password = PasswordHelper.HashPassword(user.Password); // hash 
                _db.Entry(existing).Property(u => u.Password).IsModified = true;
            }

            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Error Occured");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateOrder([FromBody] Order order)
    {
        try
        {
            var existing = await _db.Orders.FindAsync(order.Id);
            if (existing == null) return NotFound(new { success = false, message = "Order not found" });

            existing.Status = order.Status;
            await _db.SaveChangesAsync();

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error Occured");
            return StatusCode(500, new { success = false, message = "error updating order status" });
        }
    }

    // delete user
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var user = await _db.User.FindAsync(id);
            if (user == null) return NotFound(new { success = false, message = "User not found" });

            _db.User.Remove(user);
            await _db.SaveChangesAsync();

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error Occured");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
            
        }
    }
    
    
[HttpGet]
public async Task<IActionResult> GetOrders()
{
    try
    {
        var orders = await (
            from o in _db.Orders
            join u in _db.User on o.UserIdFk equals u.Id
            select new
            {
                o.Id,
                OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                Username = u.Username,
                o.ShippingAddress,
                o.TotalAmount,
                o.Status,
                ItemCount = _db.OrderItems.Count(oi => oi.OrderId == o.Id)
            }
        ).ToListAsync();

        return Ok(orders);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error Occurred");
        return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
    }
}

   
[HttpGet]
public async Task<IActionResult> GetOrderDetails(int id)
{
    try
    {
        var order = await _db.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new
            {
                o.Id,
                OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                o.ShippingAddress,
                o.TotalAmount,
                o.Status,
                Customer = _db.User
                    .Where(u => u.Id == o.UserIdFk)
                    .Select(u => (u.Firstname + " " + u.Lastname).Trim())
                    .FirstOrDefault(),

                Items = o.OrderItems.Select(i => new
                {
                    i.Id,
                    i.ProductIdfk,         
                    i.Quantity,
                    i.UnitPrice,
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (order == null)
            return NotFound();

        return Ok(order);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred fetching order details for Id {OrderId}", id);
        return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
    }
}


    // Admin/DeleteOrder
    
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
                return NotFound(new { success = false, message = "Order not found" });

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error Occured");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> DashboardStats()
    {
        try
        {
            var totalUsers = await _db.User.CountAsync();
            var totalOrders = await _db.Orders.CountAsync();
            var totalProducts = await _db.Products.CountAsync();

            var totalRevenue = await _db.Orders
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var today = DateTime.Today;
            var ordersToday = await _db.Orders
                .CountAsync(o => o.OrderDate >= today);

            return Ok(new
            {
                totalUsers,
                totalOrders,
                totalProducts,
                totalRevenue,
                ordersToday
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Occurred");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }   
    }

    [HttpGet]
    public async Task<IActionResult> RecentOrders()
    {
        try
        {
            var orders = await (
                from o in _db.Orders
                join u in _db.User on o.UserIdFk equals u.Id
                orderby o.OrderDate descending
                select new
                {
                    o.Id,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                    u.Username,
                    o.ShippingAddress,
                    o.TotalAmount,
                    ItemCount = _db.OrderItems.Count(i => i.OrderId == o.Id)
                }
            ).Take(10).ToListAsync();

            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Occurred");
            return StatusCode(500, new { success = false, message = "Unexpected error occurred" });
        }
    }

    //logout 
    [HttpGet]
    public  IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");

    }

}