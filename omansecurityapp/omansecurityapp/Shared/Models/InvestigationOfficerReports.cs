using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace omansecurityapp.Shared.Models
{

    //this model is for Investigation Officer and Admin Both

    public class InvestigationOfficerReports
    {
        public string ReportType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string AssignedOfficer { get; set; }
        public string Status { get; set; }
        public byte[] Media { get; set; }
        public string MediaType { get; set; }
    }
}
