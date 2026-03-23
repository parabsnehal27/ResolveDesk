using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResolveDesk.Models
{
    public class ComplaintAttachment
    {

        public int Id { get; set; }

        public int ComplaintId {  get; set; }

        public string FileName {  get; set; }
        public string FilePath {  get; set; }

        public string FileType { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        [ForeignKey("ComplaintId")]
        public Complaint Complaint { get; set; }
    }
}
