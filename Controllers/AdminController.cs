using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveDesk.Data;
using ResolveDesk.Models;

namespace ResolveDesk.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Dashboard()
        {
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalComplaint = _context.Complaints.Count();
            ViewBag.Pending = _context.Complaints.Count(c => c.Status == "Pending");
            return View();
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Complaints()
        {
            var complaints = _context.Complaints.OrderByDescending(c => c.CreatedAt).ToList();
            return View(complaints);
        }

        public IActionResult UpdateStatus(int id ,string status)
        {
            var complaint = _context.Complaints.FirstOrDefault(x => x.Id == id);
            if(complaint != null)
            {
                complaint.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction("Complaints");
        }

        public IActionResult DeleteComplaint(int id)
        {
            var complaint = _context.Complaints.FirstOrDefault(x => x.Id == id);
            if(complaint != null)
            {
                _context.Complaints.Remove(complaint);
                _context.SaveChanges();
            }
            return RedirectToAction("Complaints");
        }

        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }
        [HttpPost]
        public IActionResult EditUser(User model)
        {

            var user = _context.Users.FirstOrDefault(x => x.Id == model.Id);

            if (user == null)
                return NotFound();

            // Update only required fields
            user.Name = model.Name;
            user.Email = model.Email;
            user.Role = model.Role;

            // ❗ IMPORTANT: Do NOT overwrite password unless needed

            _context.SaveChanges();
            //return Content("post hi");
            return RedirectToAction("Users");
        }
        public IActionResult DeleteUser(int id)
            {
                var user = _context.Users.Find(id);

                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }

                return RedirectToAction("Users");
            }
            public IActionResult Index()
            {
                return View();
            }
    }
}
