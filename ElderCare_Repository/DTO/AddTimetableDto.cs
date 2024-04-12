using ElderCare_Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class AddTimetableDto
    {
        public DateTime? ReportDate { get; set; }

        public string? Timeframe { get; set; }

        //public int? CarerId { get; set; }

        public int ContractId { get; set; }

        public class AddTimetableTrackingDto
        {
            [RequiredWhenOtherIsNull("ContractServicesId")]
            public int? PackageServicesId { get; set; }

            [RequiredWhenOtherIsNull("PackageServicesId")]
            public int? ContractServicesId { get; set; }

            public string? Title { get; set; }
        }
        public List<AddTimetableTrackingDto>? Trackings { get; set; }
    }
}
