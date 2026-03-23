using Microsoft.AspNetCore.Mvc;
using ResolveDesk.Data;
using System.Linq;
using ResolveDesk.Models;

namespace ResolveDesk.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly AppDbContext _context;

        public ComplaintController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Submit(int departmentId)
        {
            var categories = _context.Categories.Where(c => c.DepartmentId == departmentId).ToList();

            ViewBag.Categories = categories;
            ViewBag.DepartmentId = departmentId;
            return View();
        }

        [HttpPost]
        public IActionResult Submit(string Title, string Description, string Category, int DepartmentId)
        {
            var email = User.Identity?.Name;

            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            var complaint = new Complaint
            {
                Title = Title,
                Description = Description,
                Category = Category,   // string column
                DepartmentId = DepartmentId,
                Status = "Pending",
                CreatedAt = DateTime.Now,
                StudentId = user.Id
            };

            _context.Complaints.Add(complaint);
            _context.SaveChanges();

            return RedirectToAction("MyComplaints");
        }
        [HttpGet]
        public IActionResult MyComplaints()
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            var complaints = _context.Complaints
                .Where(c => c.StudentId == user.Id)
                .ToList();

            return View(complaints);
        }
    }
}