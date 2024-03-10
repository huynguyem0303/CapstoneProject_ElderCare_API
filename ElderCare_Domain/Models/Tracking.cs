using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Domain.Models
{
    public class Tracking
    {
        public Guid TrackingId { get; set; }

        public int TimetableId { get; set; }

        public int? PackageServicesId { get; set; }

        public int? ContractServicesId { get; set; }

        public string? Title { get; set; }

        public string Image { get; set; }

        public string ReportContent { get; set; }

        public bool? CusApprove { get; set; }

        public string CusFeedback { get; set; }

        public DateTime? ReportDate { get; set; }

        public int? Status { get; set; }

        public virtual Timetable Timetable { get; set; }

        public virtual ContractService ContractServices { get; set; }

        public virtual PackageService PackageServices { get; set; }
    }
}
