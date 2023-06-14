using System.ComponentModel.DataAnnotations;

namespace omansecurityapp.Shared.Models
{
    public class AddNewReports
    {
        [Key]
        public int Id { get; set; }

        public string ReportType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public byte[] Media { get; set; }
        public string MediaType { get; set; }
    }
}
