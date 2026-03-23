using System.ComponentModel.DataAnnotations;

namespace ResolveDesk.Models
{
    public class Department
    {

        public int DepartmentId { get; set; }
        [Required]
        public string DepartmentName {  get; set; }

        //public ICollection<Complaint> complaints { get; set; }

    }
}
