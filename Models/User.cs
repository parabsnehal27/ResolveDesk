using System.ComponentModel.DataAnnotations;

namespace ResolveDesk.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        
        public string? Password { get; set; }

        [Required]
        public string Role {  get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public  ICollection<Complaint> Complaints { get; set; }
    }
}
