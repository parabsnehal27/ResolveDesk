using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResolveDesk.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Category { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int StudentId { get; set; }

        public int? DepartmentId { get; set; }

        [ForeignKey("StudentId")]
        public User Student { get; set; }

        public Department Department { get; set; }

        public ICollection<ComplaintAttachment> Attachments { get; set; }

        public ICollection<ComplaintResponse> Responses { get; set; }

    }
}
