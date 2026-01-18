using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class ProfileController(EcommerceDbContext db, ILogger<ProfileController> logger) : Controller
    {
        // GET: /Profile
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    TempData["LoginRequired"] = "Please log in to view your profile.";
                    return RedirectToAction("Index", "Home");
                }

                var user = await db.User
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId.Value);

                if (user == null)
                {
                    HttpContext.Session.Clear();
                    TempData["LoginRequired"] = "Please log in again.";
                    return RedirectToAction("Index", "Home");
                }

                var model = new ProfileViewModel
                {
                    Id = user.Id,
                    Firstname = user.Firstname ?? "",
                    Lastname = user.Lastname ?? "",
                    Username = user.Username ?? "",
                    Email = user.Email ?? "",
                    Active = user.Active,
                    UserType = user.UserType,
                    CreationDate = user.CreationDate
                };

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading profile page.");
                return StatusCode(500, "An error occurred while loading your profile.");
            }
        }

        // POST: /Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileUpdateRequest model)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                    return Unauthorized();

                if (!ModelState.IsValid)
                {
                    TempData["ProfileError"] = "Please fix the errors and try again.";
                    return RedirectToAction("Index");
                }

                var user = await db.User.FirstOrDefaultAsync(u => u.Id == userId.Value);
                if (user == null)
                    return Unauthorized();

                user.Firstname = model.Firstname.Trim();
                user.Lastname = model.Lastname.Trim();

                await db.SaveChangesAsync();

                TempData["ProfileSuccess"] = "Profile updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating profile.");
                TempData["ProfileError"] = "Could not update profile. Please try again.";
                return RedirectToAction("Index");
            }
        }
    }
}
