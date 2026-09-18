using Microsoft.AspNetCore.Mvc;
using CheckPoint2App.Data;
using CheckPoint2App.Models;

namespace CheckPoint2App.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    public IActionResult Login(User model)
    {
        // Authenticate using EmailId instead of Username
        var user = _context.Users.FirstOrDefault(u => u.EmailId == model.EmailId && u.Password == model.Password);

        if (user != null)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid Email or Password";
        return View();
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            // Check if user with same Email already exists
            if (_context.Users.Any(u => u.EmailId == user.EmailId))
            {
                ViewBag.Error = "An account with this Email ID already exists.";
                return View(user);
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        return View(user);
    }
}