using ElderCare_Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class AddContractDto
    {
        public int CarerId { get; set; }
        public int CustomerId { get; set; }
        public int ElderlyId { get; set; }
        public string?[] service { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string? PackageName { get; set; }
    }

    public class AddContractWithTrackingsDto : AddContractDto
    {
        public class TimetableDto
        {
            public DateTime? ReportDate { get; set; }

            public string? Timeframe { get; set; }

            public class TimetableTrackingDto
            {
                [RequiredWhenOtherIsNull("ContractServicesId")]
                public int? PackageServicesId { get; set; }

                [RequiredWhenOtherIsNull("PackageServicesId")]
                public int? ContractServicesId { get; set; }

                public string? Title { get; set; }
            }
            public List<TimetableTrackingDto>? Trackings { get; set; }
        }
        public List<TimetableDto> TrackingTimetables { get; set; }
    }
}
