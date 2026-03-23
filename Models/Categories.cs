using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace ResolveDesk.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public int DepartmentId {  get; set; }
        public Department Department { get; set; }

    }
}
