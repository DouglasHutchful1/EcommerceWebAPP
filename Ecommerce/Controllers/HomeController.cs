using Ecommerce.Data;
using Ecommerce.Helpers;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers;

public class HomeController(EcommerceDbContext dbcon,ILogger<HomeController> logger)  : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    //login  method
    [HttpPost]
    public async Task<IActionResult> Login(string username,string password)
    {
        try
        {

            var user = await dbcon.User.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return Unauthorized(new { message = "user not found" });
            if (!PasswordHelper.VerifyPassword(password, user.Password))
                return Unauthorized(new { message = "Wrong Password" });
            if (!user.Active)
                return Unauthorized(new { message = "User Account is inactive" });

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Email", user.Email);
            return Ok(new { success = true,message = "Login successful",user.Id});

        }
        catch (Exception e)
        {
            logger.LogError(e,"error occured");
            return StatusCode(500, new { message = "Error Occured,please try again later" });
        }
        
    }
    //method for handling resgistration
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] User requestDto)
    {
        try
        {

            if (await dbcon.User.AnyAsync(u => u.Username == requestDto.Username))
                return BadRequest(new { message = "Username already exists" });

            if (await dbcon.User.AnyAsync(u => u.Email == requestDto.Email))
                return BadRequest(new { message = "Email already exists" });

            var user = new User
            {

                Firstname = requestDto.Firstname,
                Lastname = requestDto.Lastname,
                Email = requestDto.Email,
                Username = requestDto.Username,
                Password = PasswordHelper.HashPassword(requestDto.Password),
                Active = true,
                CreationDate = DateTime.UtcNow
            };
            dbcon.User.Add(user);
            await dbcon.SaveChangesAsync();

            return Ok(new { success = true, message = "Registration was successful", user.Id });
        }
        catch (Exception e)
        {
           logger.LogError(e,"Error occured");
           return StatusCode(500, new { message = "something happened,please try again later" });
        }
        
    }
//logout 
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Logged Out" });
    }
}