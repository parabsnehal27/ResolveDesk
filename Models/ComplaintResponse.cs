using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResolveDesk.Models
{
    public class ComplaintResponse
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }

        public string ResponseMessage {  get; set; }

        public int RespondedBy { get; set; }

        public DateTime ResponseDate { get; set; } = DateTime.Now;

        [ForeignKey("ComplaintId")]
        public Complaint Complaint { get; set; }

        [ForeignKey("ResponedBy")]
        public User User { get; set; }

    }
}
