using ElderCare_Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class AddTrackingOptionDto
    {
        public int ServiceId { get; set; }
        [StringRange(AllowableValues = new[] {"text", "audio", "image"}, AllowNull = false)]
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
