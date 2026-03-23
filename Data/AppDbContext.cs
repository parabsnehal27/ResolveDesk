using Microsoft.EntityFrameworkCore;
using ResolveDesk.Models;


namespace ResolveDesk.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        
        public DbSet<Department> Departments { get; set; }

        public DbSet<Categories> Categories { get; set; }

        public DbSet<ComplaintAttachment> ComplaintAttachments { get; set;}

        public DbSet<ComplaintResponse> ComplaintResponse { get; set; }
    }
}
