using ElderCare_Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class CarerUpdateTrackingDto
    {
        public Guid TrackingId { get; set; }

        public int? TimetableId { get; set; }

        public string? Title { get; set; }

        public string? Image { get; set; }

        public string? ReportContent { get; set; }
    }
    public class CustomerApproveTrackingDto
    {
        public Guid TrackingId { get; set; }

        public int? TimetableId { get; set; }

        public string? CusFeedback { get; set; }

        [StringRange(AllowableValues = new[] {"2", "3"}, AllowNull = false)]
        public int Status { get; set; }
    }
}
