using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }

        public int? CustomerId { get; set; }

        public int? Ratng { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CarerId { get; set; }

        public int? ServiceId { get; set; }
    }
}
