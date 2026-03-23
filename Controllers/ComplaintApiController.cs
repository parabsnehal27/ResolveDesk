using Microsoft.AspNetCore.Mvc;
using ResolveDesk.Data;
using ResolveDesk.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ResolveDesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ComplaintApiController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Submit Complaint
        [Authorize(Roles = "Student")]
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitComplaint(
            [FromForm] string title,
            [FromForm] string description,
            [FromForm] string category,
            List<IFormFile> files)
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var complaint = new Complaint
            {
                Title = title,
                Description = description,
                Category = category,
                StudentId = studentId,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

            // File Upload
            if (files != null && files.Count > 0)
            {
                var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                foreach (var file in files)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var attachment = new ComplaintAttachment
                    {
                        ComplaintId = complaint.Id,
                        FileName = file.FileName,
                        FilePath = "/uploads/" + fileName,
                        FileType = file.ContentType
                    };

                    _context.ComplaintAttachments.Add(attachment);
                }

                await _context.SaveChangesAsync();
            }

            return Ok("Complaint submitted successfully");
        }

        // Student View Own Complaints
        [Authorize(Roles = "Student")]
        [HttpGet("my-complaints")]
        public IActionResult GetMyComplaints()
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var complaints = _context.Complaints
                .Where(c => c.StudentId == studentId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return Ok(complaints);
        }

        // Admin View All Complaints
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IActionResult GetAllComplaints()
        {
            var complaints = _context.Complaints
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return Ok(complaints);
        }

        // Update Complaint Status
        [Authorize(Roles = "Admin")]
        [HttpPut("update-status/{id}")]
        public IActionResult UpdateStatus(int id, [FromBody] string status)
        {
            var complaint = _context.Complaints.FirstOrDefault(x => x.Id == id);

            if (complaint == null)
                return NotFound("Complaint not found");

            complaint.Status = status;

            _context.SaveChanges();

            return Ok("Status updated successfully");
        }
    }
}