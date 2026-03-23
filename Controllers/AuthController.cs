using Microsoft.AspNetCore.Mvc;
using ResolveDesk.Data;
using ResolveDesk.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ResolveDesk.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == Email && x.Password == Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AuthCookie", principal);

            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        /*public IActionResult Login(string Email, string Password)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == Email && x.Password == Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View();
            }

            // save session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);

            // redirect based on role
            if (user.Role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }*/

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Login", "Auth");
        }

        // Register page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Register(string Name, string Email,string Password)
        {
            var exisitinguser = _context.Users.FirstOrDefault(x => x.Email == Email);

            if (exisitinguser != null)
            {
                ViewBag.Message = "User Already Exists ";
                return View();
            }

            var user = new User
            {
                Name = Name,
                Email = Email,
                Password = Password,
                Role="Student"
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            ViewBag.Message = "Registration successful";

            return RedirectToAction("Login");
        }
    }
}