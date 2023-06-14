using System.ComponentModel.DataAnnotations;

namespace omansecurityapp.Shared.Models
{
    public class PoliceOfficerReports
    {
        public string ReportType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string AssignedOfficer { get; set; }
        public byte[] Media { get; set; }
        public string MediaType { get; set; }
    }
}
