using LutongBahayApp.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.Models
{
    public class ReportIncident
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AspNetUsers")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public ReportIncidentStatus ReportIncidentStatus { get; set; } = ReportIncidentStatus.Unresolved;
    }
}
