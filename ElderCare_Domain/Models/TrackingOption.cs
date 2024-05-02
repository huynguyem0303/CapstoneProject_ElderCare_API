using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Domain.Models
{
    public class TrackingOption
    {
        public int TrackingOptionId { get; set; }
        public int ServiceId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
